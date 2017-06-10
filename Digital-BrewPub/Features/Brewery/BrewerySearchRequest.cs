using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.BrewPub.Features.Brewery
{
    public class BrewerySearchRequest
    {
        public string Term { get; set; }
        public SearchType Type { get; set; }

        public enum SearchType
        {
            City,
            Zip
        }
    }
}
