using Core.Business.Abstract;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Concrete
{
    public class RepositoryManager<TEntity, TDal> : IRepositoryService<TEntity>
         where TEntity : class, IEntity, new()
        where TDal : new()
    {
        public RepositoryManager()
        {

        }
        public void Add(TEntity entity)
        {
            
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
