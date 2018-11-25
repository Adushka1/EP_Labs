using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_3.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        int Count { get; }

        T GetEntityOrDefault(Func<T, bool> selector);
        void AddEntity(T entity);

        bool RemoveEntity(T entity);
        bool RemoveEntity(Func<T, bool> selector);

        IEnumerable<T> GetEntities(Func<T, bool> selector);
        IEnumerable<T> GetAllEntities();
    }
}
