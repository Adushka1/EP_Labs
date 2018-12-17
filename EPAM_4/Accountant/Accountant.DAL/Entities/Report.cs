using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountant.DAL.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public int ManagerId { get; set; }
        public string Customer { get; set; }
        public Manager Manager { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
