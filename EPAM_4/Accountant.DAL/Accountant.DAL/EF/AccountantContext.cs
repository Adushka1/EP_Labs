using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using Accountant.DAL.Entities;

namespace Accountant.DAL.EF
{
    public class AccountantContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }

        public AccountantContext()
        {
            Database.SetInitializer<AccountantContext>(new CreateDatabaseIfNotExists<AccountantContext>());
        }
    }
}
