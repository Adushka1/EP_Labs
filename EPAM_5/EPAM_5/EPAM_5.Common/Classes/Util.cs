﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EPAM_5.Common
{
    public static class  Util
    {
        public static void LogToFile(string path, string logText)
        {
            if (!string.IsNullOrEmpty(logText))
            {
                File.AppendAllText(path, "\r\n\r\n" + System.DateTime.Now.ToString() + "\r\n" + logText);
            }
        }

        public static string GetValueFromNvpFile(string path, string name)
        {
            string rtnValue = string.Empty;
            foreach (string line in File.ReadLines(path))
            {
                string[] parts = line.Split(new[] { '=' }, 2);
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                if (key == name)
                {
                    rtnValue = value;
                    break;
                }
            }
            return rtnValue;
        }

        public static void SaveValueToNvpFile(string path, string name, string value)
        {
            string item = string.Empty;
            List<string> lines = new List<string>();
            foreach (string line in File.ReadLines(path))
            {
                string[] parts = line.Split(new[] { '=' }, 2);
                if (parts[0].Trim() == name)
                {
                    parts[1] = value;
                    item = parts[0] + "=" + parts[1];
                }
                else
                {
                    item = line;
                }
                lines.Add(item);
            }
            File.WriteAllLines(path, lines);
        }

        public static Dictionary<string, string> GetAllFromNvpFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            var dict = lines.Select(l => l.Split(new[] { '=' }, 2)).ToDictionary(a => a[0].Trim(), a => a[1].Trim());
            return dict;

        }

        public static void SaveAllToNvpFile(string path, Dictionary<string, string> dict)
        {
            string[] lines = dict.Select(kvp => kvp.Key + "=" + kvp.Value).ToArray();
            File.WriteAllLines(path, lines);
        }

        public static List<string> GetConfigValueList(string searchPartial)
        {
            var keys = ConfigurationManager.AppSettings.Keys;
            var valueList = new List<string>();
            foreach (var key in keys)
            {
                if (key.ToString().ToLower().Contains(searchPartial.ToLower()))
                {
                    var value = ConfigurationManager.AppSettings.Get(key.ToString());
                    if (!string.IsNullOrEmpty(value)) valueList.Add(value);
                }
            }
            return valueList;
        }

        public static List<NameValue> GetConfigItemList(string searchPartial)
        {
            var keys = ConfigurationManager.AppSettings.Keys;

            var itemList = new List<NameValue>();
            var subtitle = string.Empty;
            var keyStr = string.Empty;
            int pos = 0;
            foreach (var key in keys)
            {
                if (key.ToString().ToLower().Contains(searchPartial.ToLower()))
                {
                    keyStr = key.ToString();
                    pos = keyStr.LastIndexOf("_") + 1;
                    subtitle = keyStr.Substring(pos, keyStr.Length - pos);
                    var item = new NameValue()
                    {
                        Name = subtitle,
                        Value = ConfigurationManager.AppSettings.Get(key.ToString())
                    };
                    if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Value))
                    {
                        itemList.Add(item);
                    }
                }
            }
            return itemList;
        }

        public static bool IsFileReady(String sFilename)
        {
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }        

        public static bool IsNumeric(string value)
        {
            return _isNumericRegex.IsMatch(value);
        }
        private static readonly Regex _isNumericRegex = new Regex("^(" +
             @"0x[0-9a-f]+" + "|" +
             @"0b[01]+" + "|" +
             @"0[0-7]*" + "|" +
             @"((?!0)|[-+]|(?=0+\.))(\d*\.)?\d+(e\d+)?" +
            ")$");

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        } 
    }

    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
