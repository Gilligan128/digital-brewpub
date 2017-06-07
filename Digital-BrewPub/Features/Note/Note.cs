using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.BrewPub.Features.Note
{
    public class Note
    {
        public Note()
        {
            Id = Guid.Empty;
        }

        public Guid Id { get; set; }

        public String Brewery { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }
    }
}
