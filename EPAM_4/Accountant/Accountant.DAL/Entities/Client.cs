using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountant.DAL.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private ICollection<Product> Products { get; set; }
    }
}
