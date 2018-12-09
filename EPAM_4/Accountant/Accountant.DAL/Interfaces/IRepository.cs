using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountant.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityDoDelete);
        void Update(TEntity entityToUpdate);
    }
}
