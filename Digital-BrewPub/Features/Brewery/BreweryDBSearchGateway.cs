using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Shared;
using Newtonsoft.Json;

namespace Digital.BrewPub.Features.Brewery
{
    public class BreweryDBSearchGateway : Gateway<BrewerySearchRequest, BrewerySearchResult>
    {
        public async Task<BrewerySearchResult> HandleAsync(BrewerySearchRequest request)
        {
            var httpClient = new HttpClient();
            var searchResponse = await httpClient.GetAsync("http://api.brewerydb.com/v2/locations/?key=2ae879589f6c37f97e97f56779bcd0fc&locality=Detroit");
            searchResponse.EnsureSuccessStatusCode();
            var searchResposneContent = await searchResponse.Content.ReadAsStringAsync();
            var resultsAsJson = JsonConvert.DeserializeObject<BreweryDBSearchResult>(searchResposneContent);

            var brewerySearchResult = new BrewerySearchResult
            {
                Breweries = resultsAsJson.Data.Select(brewery => new BrewerySearchResult.Brewery {
                    Name = brewery.Name,
                    StreetAddress = brewery.StreetAddress
                }
                ).ToArray()
            };
            return brewerySearchResult;
        }

        private class BreweryDBSearchResult
        {
            public BreweryDBResult[] Data { get; set; }
        }

        private class BreweryDBResult
        {
            public string Name { get;  set; }
            public string StreetAddress { get; set; }
        }
    }

    
}
