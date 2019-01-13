using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace EPAM_5.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public DateTime? AuditTime { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
