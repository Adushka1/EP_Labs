using System;

namespace EPAM_5.DAL
{
    public interface IStoreDataUnitOfWork : IDisposable
    {
        StoreDataContext Context { get; set; }
        void Commit();
        bool LazyLoadingEnabled { get; set; }
    }

}
