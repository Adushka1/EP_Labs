using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accountant.DAL.Entities;
using Accountant.DAL.Interfaces;
using Accountant.DAL.Repositories;

namespace Accountant.DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnitOfWork unit = new UnitOfWork("DbConnection");
            var mango = new Manager
            {
                Name = "Andrey"
            };
            var report = new Report
            {
                Cost = 112,
                Date = DateTime.Now,
                Manager = mango,
                ManagerId = mango.Id,
                Customer = "Ilya"
            };
            unit.ReportRepository.Insert(report);
            var t = unit.ReportRepository.Get(r => r.Cost == 112).FirstOrDefault()?.Customer;
            unit.Save();
            Console.ReadKey();
        }
    }
}
