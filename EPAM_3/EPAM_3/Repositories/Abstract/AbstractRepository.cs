using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_3.Repositories.Interfaces;

namespace EPAM_3.Repositories.Abstract
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class
    {
        public int Count => _entitySet.Count;

        private readonly ISet<T> _entitySet;

        protected AbstractRepository(IEnumerable<T> entitySet)
        {
            _entitySet = new HashSet<T>(entitySet);
        }

        public T GetEntityOrDefault(Func<T, bool> selector)
        {
            return _entitySet.FirstOrDefault(selector);
        }

        public void AddEntity(T entity)
        {
            _entitySet.Add(entity);
        }

        public bool RemoveEntity(T entity)
        {
            return _entitySet.Remove(entity);
        }

        public bool RemoveEntity(Func<T, bool> selector)
        {
            var entity = _entitySet.FirstOrDefault(selector);

            return entity != null && _entitySet.Remove(entity);
        }

        public IEnumerable<T> GetEntities(Func<T, bool> selector)
        {
            return _entitySet.Where(selector);
        }

        public IEnumerable<T> GetAllEntities()
        {
            return _entitySet;
        }
    }
}
