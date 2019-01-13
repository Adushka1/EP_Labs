using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EPAM_5.Models;

namespace EPAM_5.Common
{
    public static class GenericSorterPager
    {
        public static IOrderedQueryable<T> AsSortedQueryable<T, TSortByType>(IQueryable<T> source, string sortBy, SortDirection sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");
            Expression parent = param;
            
            string[] props = sortBy.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string prop in props)
            {
                parent = Expression.Property(parent, prop);
            }
            
            var sortExpression = Expression.Lambda<Func<T, TSortByType>>(parent, param);
            
            switch (sortDirection)
            {
                case SortDirection.Descending:
                    return source.OrderByDescending<T, TSortByType>(sortExpression);
                default:
                    return source.OrderBy<T, TSortByType>(sortExpression);
            }
        }

        public static IQueryable<T> AsPagedQueryable<T>(IQueryable<T> source, int pageIndex, int pageSize)
        {
            return source.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public static IList<T> GetSortedPagedList<T>(IQueryable<T> source, PaginationRequest paging, out int totalCount, ChildLoad childLoad = ChildLoad.None)
        {
            totalCount = 0;
            if (paging == null)
            {
                var list = source.ToList();
                totalCount = list.Count();
                return list;
            }
            source = source.Distinct();
                        
            IQueryable<T> sortedPagedQuery = GetSortedPagedQuerable<T>(source, paging);

            if (childLoad == ChildLoad.None)
            {
                var pagedGroup = from sel in sortedPagedQuery
                                 select new PagedResultSet<T>()
                                 {
                                     PagedData = sel,
                                     TotalCount = source.Count()
                                 };
                List<PagedResultSet<T>> pagedResultSet;
                try
                {
                    pagedResultSet = pagedGroup.AsParallel().ToList();
                }
                catch(NotSupportedException)
                {                    
                    totalCount = source.Count();
                    return sortedPagedQuery.ToList();
                }

                IEnumerable<T> pagedList = new List<T>();
                if (pagedResultSet.Count() > 0)
                {
                    totalCount = pagedResultSet.First().TotalCount;
                    pagedList = pagedResultSet.Select(s => s.PagedData);
                }
                pagedResultSet = null;

                return pagedList.ToList();
            }
            else 
            {
                totalCount = source.Count();
                return sortedPagedQuery.ToList();
            }
        }         
        
        private static IQueryable<T> GetSortedPagedQuerable<T>(IQueryable<T> source, PaginationRequest paging)
        {            
            IQueryable<T> pagedQuery = source;
            if (paging.Sort == null) paging.Sort = new Sort();
            string sortingPropertyType = "System.String";  
            
            PropertyInfo[] props = typeof(T).GetProperties();
            PropertyInfo sortingProperty = null;

            if (!string.IsNullOrEmpty(paging.Sort.SortBy))
            {
                if (paging.Sort.SortBy.Contains("."))
                {
                    var childSortBy = paging.Sort.SortBy.Split('.');
                    var entFullName = System.Configuration.ConfigurationManager.AppSettings["FullEntityAssemblyName"];
                    Type childT = Type.GetType(string.Format("{0}.{1}, {2}", entFullName, childSortBy[0], entFullName));
                    PropertyInfo[] childProps = childT.GetProperties();
                    sortingProperty = childProps.ToList().Find(x => x.Name.Equals(childSortBy[1]));
                }
                if (!paging.Sort.SortBy.Contains("."))
                {
                    sortingProperty = props.ToList().Find(x => x.Name.Equals(paging.Sort.SortBy));
                }
            }

            if (sortingProperty == null)            {
                paging.Sort.SortBy = props.ToList().FirstOrDefault().Name;
                sortingProperty = props.ToList().Find(x => x.Name.Equals(paging.Sort.SortBy));
            }

            sortingPropertyType = sortingProperty.PropertyType.FullName;

            if (sortingProperty.PropertyType.IsGenericType)
            {
                Type genericTypeDef = sortingProperty.PropertyType.GetGenericTypeDefinition();
                Type[] agrumentTypes = sortingProperty.PropertyType.GetGenericArguments();
                if (genericTypeDef == typeof(Nullable<>))
                {
                    sortingPropertyType = string.Format("System.Nullable`1[{0}]", agrumentTypes.FirstOrDefault().FullName);
                }
            }            
            
            if (sortingPropertyType == "System.String")
            {
                pagedQuery = AsSortedQueryable<T, String>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if (sortingPropertyType == "System.DateTime")
            {
                pagedQuery = AsSortedQueryable<T, DateTime>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if (sortingPropertyType == "System.Nullable`1[System.DateTime]")
            {
                pagedQuery = AsSortedQueryable<T, DateTime?>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if ((sortingPropertyType == "System.Nullable`1[System.Int32]") ||
                     (sortingPropertyType == "System.Nullable`1[System.Int16]") ||
                     (sortingPropertyType == "System.Nullable`1[System.Int64]"))
            {
                pagedQuery = AsSortedQueryable<T, Int32?>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if ((sortingPropertyType == "System.Nullable`1[System.Decimal]") ||
                     (sortingPropertyType == "System.Nullable`1[System.Single]") ||
                     (sortingPropertyType == "System.Nullable`1[System.Double]"))
            {
                pagedQuery = AsSortedQueryable<T, Decimal?>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if (sortingPropertyType == "System.Nullable`1[System.Boolean]")
            {
                pagedQuery = AsSortedQueryable<T, Boolean?>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if (sortingPropertyType == "System.Boolean")
            {
                pagedQuery = AsSortedQueryable<T, Boolean>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if ((sortingPropertyType == "System.Int32") ||
                     (sortingPropertyType == "System.Int16") ||
                     (sortingPropertyType == "System.Int64"))
            {
                pagedQuery = AsSortedQueryable<T, Int32>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else if ((sortingPropertyType == "System.Decimal") ||
                     (sortingPropertyType == "System.Single") ||
                     (sortingPropertyType == "System.Double"))
            {
                pagedQuery = AsSortedQueryable<T, Decimal>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            else
            {
                pagedQuery = AsSortedQueryable<T, Object>(source, paging.Sort.SortBy, paging.Sort.SortDirection);
            }
            if (paging.PageSize > 0)
            {                
                pagedQuery = AsPagedQueryable<T>(pagedQuery, paging.PageIndex, paging.PageSize);
            }
            else
            {
                paging.PageIndex = 0;
            }
            return pagedQuery;
        }
    }

    public class PagedResultSet<T>
    {
        public int TotalCount { get; set; }
        public T PagedData { get; set; }
    }

    public enum ChildLoad
    {
        Include,
        None
    }
}