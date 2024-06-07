using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities;
using Grocery.Domain.IUnitOfWork;
using Grocery.Domain.Repositories;
using Grocery.Repository.Data;
using Grocery.Repository.Repository.GenericRepository;

namespace Grocery.Repository.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly GroceryContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(GroceryContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenericRespository<TEntity> Respository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRespository<TEntity>; // if cast not found then, hashtable will return object 
        }

        public async Task<int> Complete() => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
    }
}
