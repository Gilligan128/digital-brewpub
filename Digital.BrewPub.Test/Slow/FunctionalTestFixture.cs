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

namespace Digital.BrewPub.Test.Slow
{
    public class FunctionalTestFixture : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; }

        public FunctionalTestFixture()
        {
            server =  new TestServer(new WebHostBuilder()
                 .ConfigureServices(services =>
                 {
                     services.AddMvc(options => options.Filters.Add(typeof(ConvertViewsToModelsFilter)));
                 })
                .UseStartup<Startup>());
            Client = server.CreateClient();
            
            ResetDatabase();
        }

        public void Dispose()
        {
            server.Dispose();
        }

        private static void ResetDatabase()
        {
            var checkpoint = new Checkpoint()
            {
                TablesToIgnore = new string[] { "_EFMigrationsHistory" }
            };
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            using (var dbConnection = new SqlConnection(config["ConnectionStrings:DefaultConnection"]))
            {
                dbConnection.Open();
                checkpoint.Reset(dbConnection);
            }
        }
    }
}
