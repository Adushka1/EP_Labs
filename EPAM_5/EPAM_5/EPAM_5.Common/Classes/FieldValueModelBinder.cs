using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using System.Configuration;

namespace EPAM_5.Common
{
    public class FieldValueModelBinder : IModelBinder
    {
        private const string RexChechNumeric = @"^\d+$";
        private const string RexBrackets = @"\[\d*\]";
        private const string RexSearchBracket = @"\[([^}])\]";
        
        private List<KeyValuePair<string, string>> kvps;
        
        private int maxRecursionLimit = 100;
        private int recursionCount = 0;
        
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var maxRecurseLimit = ConfigurationManager.AppSettings["MaxRecursionLimit"];                         
            if (!string.IsNullOrEmpty(maxRecurseLimit) && Regex.IsMatch(maxRecurseLimit, RexChechNumeric))
            {
                maxRecursionLimit = Convert.ToInt32(maxRecurseLimit);
            }
            
            if (!string.IsNullOrEmpty(actionContext.Request.RequestUri.Query))
            {                
                kvps = actionContext.Request.GetQueryNameValuePairs().ToList(); 
            }
            else if (actionContext.Request.Content.IsFormData())
            {                
                var bodyString = actionContext.Request.Content.ReadAsStringAsync().Result;
                try
                {
                    kvps = ConvertToKvps(bodyString);
                }
                catch (Exception ex)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
                    return false;
                }
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "No input data");
                return false;
            }            
            var obj = Activator.CreateInstance(bindingContext.ModelType);
            try
            {                
                SetPropertyValues(obj);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, ex.Message);
                return false;
            }
            bindingContext.Model = obj;
            return true;
        }
        
        public void SetPropertyValues(object obj, object parentObj = null, PropertyInfo parentProp = null)
        {           
            PropertyInfo[] props = obj.GetType().GetProperties();           
            
            List<KeyValuePair<string, string>> kvpsWork;

            foreach (var prop in props)
            {
                kvpsWork = new List<KeyValuePair<string, string>>(kvps);

                if (!prop.PropertyType.IsClass || prop.PropertyType.FullName == "System.String")
                {
                    foreach (var item in kvpsWork)
                    {
                        var key = item.Key;
                        var keyParts = Regex.Split(key, RexBrackets);
                        if (keyParts.Length > 1) key = keyParts[keyParts.Length - 1];
                        if (key == prop.Name)
                        {
                            var kvw = new KeyValueWork()
                            {
                                Key = item.Key,
                                Value = item.Value,
                                SourceKvp = item
                            };
                            AddSingleProperty(obj, prop, kvw);
                            break;
                        }
                    }   
                }
                else if (prop.PropertyType.IsClass)
                {
                    if (prop.ToString().Contains("[System.String]") || prop.ToString().Contains("System.String[]"))
                    {
                        var strList = new List<string>();
                        foreach (var item in kvpsWork)
                        {
                            var itemKey = Regex.Replace(item.Key, RexBrackets, "");
                            if (itemKey == prop.Name)
                            {
                                strList.Add(item.Value);
                                kvps.Remove(item);
                            }
                        }
                        if (prop.PropertyType.IsGenericType) prop.SetValue(obj, strList);
                        else if (prop.PropertyType.IsArray) prop.SetValue(obj, strList.ToArray()); 
                    }
                    else
                    {
                        RecurseNestedObj(obj, prop);
                    }
                }
            }
            if (parentObj != null)
            {                
                parentProp.SetValue(parentObj, obj, null);                
            } 
        }

        private void RecurseNestedObj(object obj, PropertyInfo prop, string pParentName = "", string pParentObjIndex = "")
        {
            if (recursionCount > maxRecursionLimit)
            {
                throw new Exception(string.Format("Exceed maximum recursion limit {0}", maxRecursionLimit));
            }
            recursionCount++;

            if (prop.PropertyType.IsGenericType || prop.PropertyType.BaseType.IsGenericType)
            {
                if ((prop.PropertyType.IsGenericType && prop.PropertyType.Name != "List`1")
                    || (prop.PropertyType.BaseType.IsGenericType && prop.PropertyType.BaseType.Name != "List`1"))
                {
                    throw new Exception("Only support nested Generic List collection");
                }
                if (prop.PropertyType.GenericTypeArguments.Count() > 1 || prop.PropertyType.BaseType.GenericTypeArguments.Count() > 1)
                {
                    throw new Exception("Only support nested Generic List collection with one argument");
                }
            }

            object childObj = null;
            if (prop.PropertyType.IsGenericType || prop.PropertyType.BaseType.IsGenericType || prop.PropertyType.IsArray)
            {
                if (prop.PropertyType.IsGenericType)
                {
                    childObj = Activator.CreateInstance(prop.PropertyType.GenericTypeArguments[0]);

                }
                else if (!prop.PropertyType.IsGenericType && prop.PropertyType.BaseType.IsGenericType)
                {
                    childObj = Activator.CreateInstance(prop.PropertyType.BaseType.GenericTypeArguments[0]);
                }
                else if (prop.PropertyType.IsArray)
                {
                    childObj = Activator.CreateInstance(prop.PropertyType.GetElementType());
                }
                SetPropertyValuesForList(childObj, parentObj: obj, parentProp: prop,
                            pParentName: pParentName, pParentObjIndex: pParentObjIndex);
            }
            else
            {
                childObj = Activator.CreateInstance(prop.PropertyType);
                SetPropertyValues(childObj, parentObj: obj, parentProp: prop);
            }
        }
               
        private void SetPropertyValuesForList(object obj, object parentObj = null, PropertyInfo parentProp = null, 
                                             string pParentName = "", string pParentObjIndex = "")
        {
            PropertyInfo[] props = obj.GetType().GetProperties();
            List<KeyValueWork> kvwsGroup = new List<KeyValueWork>();            
            List<List<KeyValueWork>> kvwsGroups = new List<List<KeyValueWork>>();
            
            Regex regex;
            Match match;
            bool isGroupAdded = false;
            string lastIndex = "";
            
            foreach (var item in kvps)
            {                               
                if (item.Key.Contains(parentProp.Name))
                {
                    if (pParentName != "" & pParentObjIndex != "")
                    {
                        regex = new Regex(pParentName + RexSearchBracket);
                        match = regex.Match(item.Key);
                        if (match.Groups[1].Value != pParentObjIndex)
                            break;
                    }
                    regex = new Regex(parentProp.Name + RexSearchBracket);
                    match = regex.Match(item.Key);
                    var brackets = match.Value.Replace(parentProp.Name, "");
                    var objIdx = match.Groups[1].Value;

                    if (lastIndex != "" && objIdx != lastIndex)
                    {
                        kvwsGroups.Add(kvwsGroup);
                        isGroupAdded = true;
                        kvwsGroup = new List<KeyValueWork>();
                    }
                    var keyParts = item.Key.Split(new string[] { brackets }, StringSplitOptions.RemoveEmptyEntries);
                    var kvw = new KeyValueWork()
                    {
                        ObjIndex = objIdx,
                        ParentName = parentProp.Name,
                        Key = keyParts[keyParts.Length - 1],
                        Value = item.Value,
                        SourceKvp = item
                    };
                    kvwsGroup.Add(kvw);
                    lastIndex = objIdx;
                    isGroupAdded = false;                    
                }                
            }
            if (kvwsGroup.Count > 0 && isGroupAdded == false)
                kvwsGroups.Add(kvwsGroup);

            IList listObj = null;
            Array arrayObj = null;
            if (parentProp.PropertyType.IsGenericType || parentProp.PropertyType.BaseType.IsGenericType)
            {
                listObj = (IList)Activator.CreateInstance(parentProp.PropertyType);
            }
            else if (parentProp.PropertyType.IsArray)
            {
                arrayObj = Array.CreateInstance(parentProp.PropertyType.GetElementType(), kvwsGroups.Count);
            }    
                       
            int idx = 0;
            foreach (var group in kvwsGroups)
            {
                object tempObj = null;                
                
                tempObj = Activator.CreateInstance(obj.GetType());
                foreach (var prop in props)
                {
                    if (!prop.PropertyType.IsClass || prop.PropertyType.FullName == "System.String")
                    {
                        foreach (var item in group)
                        {
                            if (item.Key == prop.Name)
                            {
                                AddSingleProperty(tempObj, prop, item);
                                break;
                            }
                        }
                    }
                    else if (prop.PropertyType.IsClass)
                    {
                        if (prop.ToString().Contains("[System.String]") || prop.ToString().Contains("System.String[]"))
                        {
                            var tempProps = tempObj.GetType().GetProperties();                               
                                
                            foreach (var tempProp in tempProps)
                            {
                                if (tempProp.Name == prop.Name)
                                {                                   
                                    var strList = new List<string>();
                                                                        
                                    foreach (var item in group)
                                    {
                                        var itemKey = Regex.Replace(item.Key, RexBrackets, "");
                                        if (itemKey == tempProp.Name)
                                        {
                                            strList.Add(item.Value);
                                            kvps.Remove(item.SourceKvp);
                                        }
                                    }
                                    if (prop.PropertyType.IsGenericType) tempProp.SetValue(tempObj, strList);
                                    else if (prop.PropertyType.IsArray) tempProp.SetValue(tempObj, strList.ToArray());                                    
                                }
                            }
                        }
                        else
                        {
                            RecurseNestedObj(tempObj, prop, pParentName: group[0].ParentName, pParentObjIndex: group[0].ObjIndex);
                        }
                    }                    
                }
                
                if (listObj != null)
                {                    
                    listObj.Add(tempObj);
                }
                else if (arrayObj != null)
                {                    
                    arrayObj.SetValue(tempObj, idx);
                    idx++;
                }                
            }
            if (listObj != null)
            {
                parentProp.SetValue(parentObj, listObj, null);
            }
            else if (arrayObj != null)
            {
                parentProp.SetValue(parentObj, arrayObj, null);
            }            
        }

        private void AddSingleProperty(object obj, PropertyInfo prop, KeyValueWork item)
        {            
            if (prop.PropertyType.IsEnum)
            {
                var enumValues = prop.PropertyType.GetEnumValues();
                object enumValue = null;
                bool isFound = false;
                
                for (int i = 0; i < enumValues.Length; i++)
                {                    
                    if (item.Value.ToLower() == enumValues.GetValue(i).ToString().ToLower())
                    {
                        enumValue = enumValues.GetValue(i);
                        isFound = true;
                        break;
                    }
                }
                if(!isFound)
                {
                    for (int i = 0; i < enumValues.Length; i++)
                    {
                        if (item.Value == i.ToString())
                        {
                            enumValue = i;                            
                            break;
                        }
                    }
                }                
                prop.SetValue(obj, enumValue, null);
            }
            else
            {
                prop.SetValue(obj, Convert.ChangeType(item.Value, prop.PropertyType), null);
            }
            kvps.Remove(item.SourceKvp);
        }
                
        private List<KeyValuePair<string, string>> ConvertToKvps(string sourceString)
        {
            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();
            if (sourceString.StartsWith("?")) sourceString = sourceString.Substring(1);
            string[] elements = sourceString.Split('=', '&');            
            for (int i = 0; i < elements.Length; i += 2)
            {
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>
                ( 
                    elements[i],
                    elements[i + 1]
                );
                kvpList.Add(kvp);
            }
            return kvpList;
        }

        private class KeyValueWork
        {
            internal string ObjIndex { get; set; }
            internal string ParentName { get; set; }
            internal string Key { get; set; }
            internal string Value { get; set; }
            internal KeyValuePair<string, string> SourceKvp { get; set; }
        }
    }   
}
