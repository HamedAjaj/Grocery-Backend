using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Domain.Entities;
using Grocery.Domain.Specifications;

namespace Grocery.Repository
{
    public class SpecificationEvaluator<TEntity>  where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> spec)
        {
            var query = inputQuery; //_dbContext.product
            if( spec.Criteria is not null)
                query = query.Where(spec.Criteria); // _dbContext.product.where(p => p.id==id) or another

            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if(spec.IsPaginationEnabled)
                query=query.Skip(spec.Skip).Take(spec.Take);
                
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            // _dbContext.product.where(p => p.id == id).Include(p=>p.fk).Include(p=>p.fk)...
            return query; 

        }
    }
}
