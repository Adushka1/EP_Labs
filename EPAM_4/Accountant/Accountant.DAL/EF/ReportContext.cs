using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accountant.DAL.Entities;

namespace Accountant.DAL.EF
{
    public class ReportContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Manager> Clients { get; set; }

        public ReportContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ReportContext>());
        }

        public ReportContext(string connectionString) : base("DBConnection")
        {
        }

    }
}
