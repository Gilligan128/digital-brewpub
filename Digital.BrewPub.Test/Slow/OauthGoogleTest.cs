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
    [Collection("1")]
    public class OauthGoogleTest
    {
       [Fact]
       public async Task AuthenticatesUserWithGoogle()
        {

            using(var fixture = new FunctionalTestFixture())
            {
                var client = fixture.Client;
                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                { "provider", "Google" }
                });

                var response = await client.PostAsync("/Account/ExternalLogin", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                HttpRequestMessage googleTokenRequest = new HttpRequestMessage
                {
                   RequestUri = response.Headers.Location,
                   Method = HttpMethod.Get,
                };
                googleTokenRequest.Headers.Add("Set-Cookie",  response.Headers.GetValues("Set-Cookie"));
                
                var secondResponse = await client.SendAsync(googleTokenRequest);
                
                response.StatusCode.Should().Be(HttpStatusCode.Redirect);
                response.Headers.Location.AbsoluteUri.Contains("https://accounts.google.com/o/oauth2/auth?response_type=code").Should().BeTrue();

            }
          
        }

    }
}
