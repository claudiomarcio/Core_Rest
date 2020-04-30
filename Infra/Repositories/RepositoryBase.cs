using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Infra.EntityConfiguration;
using Infra.Interfaces.Repositories.RepositoryBase;




namespace Infra.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {

        private readonly ApplicationDbContext _contex;
        public RepositoryBase(ApplicationDbContext contex)
           => _contex = contex;

        public void BulkInsertList(List<TEntity> list) => _contex.BulkInsert(list);

        public void BulkUpdateList(List<TEntity> list)
        {
            _contex.BulkUpdate(list);
  
        }
            
        public void BulkInsertOrUpdate(List<TEntity> list) => _contex.BulkInsertOrUpdate(list);


        public TEntity Add(TEntity obj)
        {
            _contex.Add(obj);
            _contex.SaveChanges();
            return obj;
        }

        public async Task <TEntity> AddAsync(TEntity obj)
        {
          await _contex.AddAsync(obj);
          await _contex.SaveChangesAsync();
          return  obj;
        }


        public List<TEntity> AddRange(List<TEntity> obj)
        {
            _contex.AddRange(obj);
            _contex.SaveChanges();
            return obj;
        }

        public IEnumerable<TEntity> GetAll()
            => _contex.Set<TEntity>().ToList();

        public TEntity GetById(int id)
             => _contex.Set<TEntity>().Find(id);

        public TEntity GetById(Guid id)
            => _contex.Set<TEntity>().Find(id);

        public void Remove(TEntity obj)
        {
            _contex.Remove(obj);
            _contex.SaveChanges();
        }

        //public void SaveChanges()
        //    => _contex.SaveChanges();

        public void Update(TEntity obj)
        {
            _contex.Set<TEntity>().Attach(obj);
            _contex.SaveChanges();
        }


        public virtual Task UpdateAsync(TEntity obj)
        {
            _contex.Set<TEntity>().Attach(obj);
            _contex.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public virtual Task UpdateCollectionAsync(IEnumerable<TEntity> entities)
        {
            _contex.UpdateRange(entities);
            return Task.CompletedTask;
        }

        //public virtual async Task AddAsync(TEntity entity)
        //{
        //    await _contex.AddAsync(entity).ConfigureAwait(false);
        //}

        public virtual async Task AddCollectionAsync(IEnumerable<TEntity> entities)
        {
            await _contex.AddRangeAsync(entities).ConfigureAwait(false);
        }

        public void UpdateRange(List<TEntity> list)
        {
            try
            {
                _contex.Set<TEntity>().AttachRange(list);
                _contex.SaveChanges();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
        }

    }
}