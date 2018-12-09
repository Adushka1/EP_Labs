using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountant.DAL.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
    }
}
