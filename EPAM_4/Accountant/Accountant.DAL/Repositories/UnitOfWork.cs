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

        private ReportContext _context;
        private GenericRepository<Report> reportRepository;
        private GenericRepository<Manager> managerRepository;

        public UnitOfWork(string connectionString)
        {
            _context = new ReportContext(connectionString);
        }

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

        public GenericRepository<Manager> ManagerRepository
        {
            get
            {

                if (this.managerRepository == null)
                {
                    this.managerRepository = new GenericRepository<Manager>(_context);
                }
                return managerRepository;
            }
        }

        public  void Save()
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
