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
            IUnitOfWork unit = new UnitOfWork();
            var client = new Client
            {
                Id = 1,
                Name = "vova"
            };
            var report = new Report
            {
                Client = client,
                Date = DateTime.Now,
                Id = 2,
                Cost = 112
            };

            unit.ReportRepository.Insert(report);
            Console.ReadKey();
        }
    }
}
