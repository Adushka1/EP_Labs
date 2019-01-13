using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace EPAM_5.DAL
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IList<TEntity> GetAll();
        TEntity GetById(object id);
        IQueryable<TEntity> GetIQueryable();
        IList<TEntity> GetAllPaged(int pageNumber, int pageSize, out int totalCount);
        object Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void CommitAllChanges();
        void Dispose();
    }

}
