using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accountant.DAL.Entities;
using Accountant.DAL.Repositories;

namespace Accountant.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<Report> ReportRepository { get; }
        GenericRepository<Client> ClientRepository { get; }
        GenericRepository<Product> ProductRepository { get; }

        void Save();
    }
}
