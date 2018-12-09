using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accountant.DAL.EF;
using Accountant.DAL.Entities;
using Accountant.DAL.Interfaces;

namespace Accountant.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private ReportContext _context = new ReportContext();
        private GenericRepository<Report> reportRepository;
        private GenericRepository<Client> clientRepository;
        private GenericRepository<Product> productRepository;

        public GenericRepository<Report> ReportRepository
        {
            get
            {
                if (this.reportRepository == null)
                {
                    this.reportRepository = new GenericRepository<Report>(_context);
                }
                return reportRepository;
            }
        }

        public GenericRepository<Client> ClientRepository
        {
            get
            {

                if (this.clientRepository == null)
                {
                    this.clientRepository = new GenericRepository<Client>(_context);
                }
                return clientRepository;
            }
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {

                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<Product>(_context);
                }
                return productRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
