
using Grocery.Domain.Entities.Identity;
using Grocery.Extensions;
using Grocery.Repository.Data;
using Grocery.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Grocery
{
    public class Program
    {
        public static async Task  Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("StoreConnection") ??
                                throw new InvalidOperationException("Error in Database Connection");
            builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(connectionString));


            builder.Services.AddDbContext<AppIdentityDbContext>(opions =>
            {
                opions.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
        
            var app = builder.Build();


            // Execute  update-database automatically without make it manually  
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>();
                await dbContext.Database.MigrateAsync();

                //  Seed Data
                await StoreContextSeed.SeedAsync(dbContext);

                var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during  apply the migration ");
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
