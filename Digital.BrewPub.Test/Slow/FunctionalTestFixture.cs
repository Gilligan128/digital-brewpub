using System;
using System.Net.Http;
using Digital.BrewPub.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace Digital.BrewPub.Test.Slow
{
    public class FunctionalTestFixture : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; }

    
        public FunctionalTestFixture()
        {
            server = new TestServer(new WebHostBuilder()
                 .ConfigureServices(services =>
                 {
                     //Even though this says "AddMVC" and not "EnsureMVC" it actually is cumulative with the Startup AddMvc
                     services.AddMvc(options => { 
                         options.Filters.Add(typeof(ConvertViewsToModelsFilter));
                         });
                 })
                .UseStartup<Startup>());
            Client = server.CreateClient();

            ResetDatabase();
        }

        public void WithinDbContext(Action<ApplicationDbContext> action)
        {
            using (var appDbContext = createDbContext())
            {
                action(appDbContext);
            };
        }

        public void Dispose()
        {
            server.Dispose();
        }

        private  void ResetDatabase()
        {
            var checkpoint = new Checkpoint()
            {
                TablesToIgnore = new string[] { "__EFMigrationsHistory" }
            };

            WithinDbContext(dbContext =>
            {
                dbContext.Database.Migrate();
                dbContext.Database.OpenConnection();
                checkpoint.Reset(dbContext.Database.GetDbConnection());
                dbContext.Database.CloseConnection();
            });
           
        }

        private  ApplicationDbContext createDbContext()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = config["ConnectionStrings:DefaultConnection"];
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString);
            ApplicationDbContext applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            return applicationDbContext;
        }
    }
}
