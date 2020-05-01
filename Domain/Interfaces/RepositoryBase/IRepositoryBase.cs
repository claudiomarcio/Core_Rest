using System;
using System.Collections.Generic;

namespace Domain.Interfaces.Repositories.RepositoryBase
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        TEntity Add(TEntity obj);
        TEntity GetById(int id);
        TEntity GetById(Guid id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(TEntity obj);

        
    }
}