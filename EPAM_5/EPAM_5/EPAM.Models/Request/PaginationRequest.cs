using System;
using System.Collections.Generic;

namespace EPAM_5.Models
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class PaginationRequest
    {        
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Sort Sort { get; set; }
    }

    public class Sort
    {
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }  
    }    
}
