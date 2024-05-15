using Grocery.Domain.Entities;
using Grocery.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Domain
{
    public interface IUnitOfWork :IAsyncDisposable
    {
        IGenericRespository<TEntity> Respository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}
