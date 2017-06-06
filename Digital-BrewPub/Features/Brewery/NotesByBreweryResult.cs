namespace Digital.BrewPub.Features.Brewery
{
    public class NotesByBreweryResult
    {
        public Note[] Notes { get; set; }

        public class Note
        {
            public Note()
            {
                AuthorId = "";
                Text = "";
                Brewery = "";
            }

            public string Text { get; set; }
            public string Brewery { get; set; }
            public string AuthorId { get; set; }
        }
    }
}