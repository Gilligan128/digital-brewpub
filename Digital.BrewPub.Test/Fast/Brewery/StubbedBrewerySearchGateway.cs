using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Brewery;
using Digital.BrewPub.Features.Shared;

namespace Digital.BrewPub.Test.Fast.Brewery
{
    internal class StubbedBrewerySearchGateway : Gateway<BrewerySearchRequest, BrewerySearchResult>
    {
        private ISet<BrewerySearchResult.Brewery> breweries = new HashSet<BrewerySearchResult.Brewery>();

        public Task<BrewerySearchResult> HandleAsync(BrewerySearchRequest request)
        {
            return Task.FromResult(new BrewerySearchResult
            {
                Breweries = new List<BrewerySearchResult.Brewery>(breweries).ToArray()
            });
        }

        internal void AddBrewery(BrewerySearchResult.Brewery brewery)
        {
            breweries.Add(brewery);
        }
    }
}