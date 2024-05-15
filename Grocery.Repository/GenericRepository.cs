using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities;
using Grocery.Domain.Repositories;
using Grocery.Domain.Specifications;
using Grocery.Repository.Data;

namespace Grocery.Repository
{
    public class GenericRepository<T> : IGenericRespository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext) // Ask clr to create object from dbcontext implicitly
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
                return (IReadOnlyList<T>) await _dbContext.Products.Include(p=>p.ProductBrand).Include(p=>p.ProductType).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }

      

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }


        public Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }


        public async Task Add(T entity) => await _dbContext.Set<T>().AddAsync(entity);
        public void Update(T entity) =>  _dbContext.Set<T>().Update(entity);
        public void Delete(T entity)   => _dbContext.Set<T>().Remove(entity); // using this code data is deleted , but i used saveChanges then is DeAtatched


        /*انا بعمل الfunction دي عشان مكررش نفس الكود */
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

    }
}
