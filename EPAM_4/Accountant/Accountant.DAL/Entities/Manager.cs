using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountant.DAL.Entities
{
    public class Manager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private ICollection<Report> Reports { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
