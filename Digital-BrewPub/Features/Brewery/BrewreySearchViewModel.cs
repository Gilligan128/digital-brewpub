using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.BrewPub.Features.Brewery
{
    public class BrewerySearchViewModel
    {
        public BrewerySearchViewModel()
        {
            Breweries = new Brewery[] { };
        }
        public Brewery[] Breweries { get; set; }

        public class Brewery
        {
            public Brewery()
            {   
                Notes = new NotesByBreweryResult.Note[] { };
            }

            public string Name { get; set; }
            public string StreetAddress { get; set; }
            public NotesByBreweryResult.Note[] Notes { get; set; }

           
        }
    }
}
