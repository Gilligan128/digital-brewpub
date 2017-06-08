using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.BrewPub.Features.Brewery
{
    public class BrewerySearchResult
    {
        public Brewery[] Breweries { get; set; }

        public class Brewery
        {
            public string Name { get; set; }
            public string StreetAddress { get; set; }
            public string NaturalKey { get;  set; }
        }
    }
}
