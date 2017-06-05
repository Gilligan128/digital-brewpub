using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Digital.BrewPub.Test
{
    public class BreweryDBClassificationTest
    {
        [Fact]
        [Trait("Category","Classification")]
        public async Task GetsSearchResultsWithZipcode()
        {
            var client = new HttpClient();

            var response = await client.GetAsync("http://api.brewerydb.com/v2/locations/?key=2ae879589f6c37f97e97f56779bcd0fc&locality=Detroit");
            var responseContent = await response.Content.ReadAsStringAsync();

            responseContent.Should().BeEmpty();
        }
    }
}
