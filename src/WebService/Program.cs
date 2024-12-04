
using ClickHouse.Client.ADO;
using ClickHouse.EntityFrameworkCore.Extensions;

using DatabaseClickhouse;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Reflection.Metadata;

namespace WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddOptions();

            builder.Services.Configure<ClickhouseConnectionParameters>(builder.Configuration.GetSection("ClickhouseConnection"));

            // Clickhouse data
            builder.Services.AddDbContextPool<ClickhouseDbContext>(
                (IServiceProvider provider, DbContextOptionsBuilder c) =>
                {
                    var connectionOptions = provider.GetRequiredService<IOptions<ClickhouseConnectionParameters>>();

                    var parameters = connectionOptions.Value;
                    if (parameters is null)
                    {
                        parameters = ClickhouseConnectionParameters.Default;
                    }

                    // clickhouse in local docker
                    ClickHouseConnectionStringBuilder b = new()
                    {
                        Host = parameters.Host,
                        Port = parameters.Port,
                        Username = parameters.Username,
                        Password = parameters.Password,
                        Database = parameters.Database
                    };
                    var connectionString = b.ToString();
                    c.UseClickHouse(connectionString, x =>
                    {

                    });
                });

            var app = builder.Build();
            // App container ready

            // Logging EF+Driver versions
            app.Logger.LogInformation("EntityFramework {0}",
                typeof(Microsoft.EntityFrameworkCore.DbContext).Assembly.FullName);

            app.Logger.LogInformation("EntityFrameworkClickhouse {0}",
                typeof(Microsoft.EntityFrameworkCore.ClickHouseEntityTypeBuilderExtensions).Assembly.FullName);

            app.Logger.LogInformation("ClickhouseClient {0}",
                typeof(ClickHouse.Client.IClickHouseConnection).Assembly.FullName);


            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                OnStart(scope);
            }
            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }


        private static void OnStart(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ClickhouseDbContext>();

            var items = dbContext.Sales.Where(x => x.Country == "US")
                .GroupBy(x => x.Region)
                .Select(x => new
                {
                    x.Key,
                    Sales = x.Sum(y => y.Sales),
                })
                .ToArray();

            foreach(var item in items)
            {
                Console.WriteLine("{0} - {1}", item.Key, item.Sales);
            }
        }
    }
}