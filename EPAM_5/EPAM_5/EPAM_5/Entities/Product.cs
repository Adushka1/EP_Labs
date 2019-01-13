using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace EPAM_5.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public Decimal? UnitPrice { get; set; }
        public int? StatusCode { get; set; }
        public System.DateTime? AvailableSince { get; set; }
        public System.DateTime? AuditTime { get; set; }

        public virtual Category Category { get; set; }
        public virtual ProductStatusType ProductStatusType { get; set; }
    }    
}
