using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital.BrewPub.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Digital.BrewPub.Features.Brewery
{
    public class BreweryController : Controller
    {
        private Gateway<BrewerySearchRequest, BrewerySearchResult> searchHandler;

        public BreweryController(Gateway<BrewerySearchRequest, BrewerySearchResult> searchGateway)
        {
            this.searchHandler = searchGateway;
        }

        public async Task<IActionResult> Search()
        {
            var brewerySearchResults = await searchHandler.HandleAsync(new BrewerySearchRequest()); // I broke YAGNI a bit for this iteration
            return View(brewerySearchResults.Breweries);
        }
    }
}