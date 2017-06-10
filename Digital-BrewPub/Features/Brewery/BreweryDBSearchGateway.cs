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
        IDictionary<BrewerySearchRequest.SearchType, string> searchTypes = new Dictionary<BrewerySearchRequest.SearchType, string>
        {
            {BrewerySearchRequest.SearchType.City, "locality" },
            {BrewerySearchRequest.SearchType.Zip, "postalCode" }
        };

        public async Task<BrewerySearchResult> HandleAsync(BrewerySearchRequest request)
        {
            var searchKey = this.searchTypes[request.Type];
            var httpClient = new HttpClient();
            //In a secure app the key would be in a secure storage.
            var searchResponse = await httpClient.GetAsync($"http://api.brewerydb.com/v2/locations/?key=2ae879589f6c37f97e97f56779bcd0fc&{searchKey}={request.Term}");
            searchResponse.EnsureSuccessStatusCode();
            var searchResposneContent = await searchResponse.Content.ReadAsStringAsync();
            var resultsAsJson = JsonConvert.DeserializeObject<BreweryDBSearchResult>(searchResposneContent);

            var brewerySearchResult = new BrewerySearchResult
            {
                Breweries = resultsAsJson.Data.Select(brewery => new BrewerySearchResult.Brewery {
                    NaturalKey = (brewery.Brewery.Name ?? "").Replace(" ", ""),
                    Name = brewery.Brewery.Name,
                    StreetAddress = brewery.StreetAddress
                }
                ).ToArray()
            };
            return brewerySearchResult;
        }

        private class BreweryDBSearchResult
        {
            public BreweryDBSearchResult()
            {
                Data = new BreweryDBResult[] { }
;            }
            public BreweryDBResult[] Data { get; set; }
        }

        private class BreweryDBResult
        {
            public string Name { get;  set; }
            public string StreetAddress { get; set; }
            public BreweryDBBrewery Brewery { get; set; }
        }

        private class BreweryDBBrewery
        {
            public string Name { get; set; }
        }
    }

    
}
