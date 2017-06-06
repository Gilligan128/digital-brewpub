using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Digital.BrewPub.Data;
using Microsoft.EntityFrameworkCore;

namespace Digital.BrewPub.Test.Slow
{
    public class FunctionalTestFixture : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; }
        public ApplicationDbContext DbContext { get;  }

        public FunctionalTestFixture()
        {
            server = new TestServer(new WebHostBuilder()
                 .ConfigureServices(services =>
                 {
                     services.AddMvc(options => options.Filters.Add(typeof(ConvertViewsToModelsFilter)));
                 })
                .UseStartup<Startup>());
            Client = server.CreateClient();

            DbContext = createDbContext();

            ResetDatabase();
        }

        public void Dispose()
        {
            server.Dispose();
            DbContext.Dispose();
        }

        private  void ResetDatabase()
        {
            var checkpoint = new Checkpoint()
            {
                TablesToIgnore = new string[] { "_EFMigrationsHistory" }
            };

            DbContext.Database.OpenConnection();
            checkpoint.Reset(DbContext.Database.GetDbConnection());
            DbContext.Database.CloseConnection();
        }

        private static ApplicationDbContext createDbContext()
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
