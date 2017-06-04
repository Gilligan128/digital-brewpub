using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Respawn;
using Xunit;

namespace Digital.BrewPub.Test.Slow
{
    public class OauthGoogleTest
    {
       [Fact]
       public async Task AuthenticatesUserWithGoogle()
        {
            resetDatabase();

            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                { "provider", "Google" }
            });

            var response = await client.PostAsync("/Account/ExternalLogin", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        private static void resetDatabase()
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
