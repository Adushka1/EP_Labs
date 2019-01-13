using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EPAM_5.Entities;
using EPAM_5.DAL;

namespace EPAM_5.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public IStoreDataUnitOfWork UnitOfWork { get; set; }
        public GenericRepository(IStoreDataUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
                
        private IDbSet<TEntity> _objectset;
        internal IDbSet<TEntity> DbSet
        {
            get
            {
                if (_objectset == null)
                {
                    _objectset = this.UnitOfWork.Context.Set<TEntity>();
                }
                return _objectset;
            }
        }

        #region IRepository<TEntity> Members
        public virtual IList<TEntity> GetAll()
        {
            return this.DbSet.ToList();
        }

        public virtual TEntity GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual IQueryable<TEntity> GetIQueryable()
        {
            return this.DbSet.AsQueryable<TEntity>();
        }

        public virtual IList<TEntity> GetAllPaged(int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = this.DbSet.Count();
            return this.DbSet.Skip(pageSize * pageIndex).Take(pageSize).ToList();
        }

        public virtual object Insert(TEntity entity)
        {
            return this.DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            this.DbSet.Remove(GetById(id));
        }

        public virtual void Delete(TEntity entity)
        {
            this.DbSet.Attach(entity);
            this.DbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this.DbSet.Attach(entity);
            this.UnitOfWork.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void CommitAllChanges()
        {
            this.UnitOfWork.Commit();
        }

        public virtual void Dispose()
        {
            this.UnitOfWork.Dispose();
        }
        #endregion
    }
}
