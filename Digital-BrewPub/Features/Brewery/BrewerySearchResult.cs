using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.BrewPub.Features.Brewery
{
    public class BrewerySearchResult
    {
        public BrewerySearchResult()
        {
            Breweries = new Brewery[] { };
        }

        public  Brewery[] Breweries { get; set; }

        public class Brewery
        {
            public string Name { get; set; }
            public string StreetAddress { get; set; }
        }
    }
}
