
using Grocery.Domain.Entities;
using Grocery.Domain.Entities.Identity;
using Grocery.Extensions;
using Grocery.Repository.Data;
using Grocery.Repository.Identity;
using Grocery.Service.ServiceDependancies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Stripe.Tax;
using System.Threading.RateLimiting;

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
            builder.Services.AddServicesDependancies();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("GroceryPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200") // Angular development server
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("GroceryPolicy", options =>
            //    {
            //        options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            //    });
            //});

            builder.Services.AddIdentityServices(builder.Configuration);

            // Rate Limiter for endpoints requests 
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", options =>
                {
                    options.PermitLimit = 5;
                    options.Window = TimeSpan.FromSeconds(10);
                    options.QueueLimit = 2;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

                });
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Grocery API v1" ,
                    Version = "v1"
                });
            });

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
         
            var connectionString = builder.Configuration.GetConnectionString("GroceryConnection") ??
                                throw new InvalidOperationException("Error in Database Connection");
            builder.Services.AddDbContext<GroceryContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<AppIdentityDbContext>(opions =>
            {
                opions.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            
            var app = builder.Build();


            // Execute  update-database automatically without make it manually  
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<GroceryContext>();
                await dbContext.Database.MigrateAsync();

                //  Seed Data
                await GroceryContextSeed.SeedAsync(dbContext);

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
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                //c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grocery API v1")
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRateLimiter();


            app.UseCors("GroceryPolicy");

            app.UseRouting();
        //    app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

           
        
        app.Run();
        }
    }
}
