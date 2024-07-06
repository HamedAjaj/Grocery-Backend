using Grocery.Repository.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Tests.DBContexts.Test
{
    public class InMemoryIdentityDbContextWithUserActions : IDisposable
    {

        private readonly AppIdentityDbContext _IdentityDb;

        public InMemoryIdentityDbContextWithUserActions()
        {
            var options = new DbContextOptionsBuilder<AppIdentityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _IdentityDb = new AppIdentityDbContext(options);
        }
        public void Dispose()
        {
            _IdentityDb.Database.EnsureDeleted();
           _IdentityDb.Dispose();
        }


    }
}
