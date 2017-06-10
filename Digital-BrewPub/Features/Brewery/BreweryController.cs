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
        private readonly Gateway<BrewerySearchRequest, BrewerySearchResult> searchHandler;
        private readonly HandleQuery<NotesByBreweryQuery, NotesByBreweryResult> notesForBreweriesQuery;

        public BreweryController(Gateway<BrewerySearchRequest, BrewerySearchResult> searchGateway, HandleQuery<NotesByBreweryQuery, NotesByBreweryResult> notesForBreweriesQuery)
        {
            this.searchHandler = searchGateway;
            this.notesForBreweriesQuery = notesForBreweriesQuery;
        }

        public async Task<IActionResult> Search(BrewerySearchRequest form)
        {
            var brewerySearchResults = await searchHandler.HandleAsync(form);
            var notes = await notesForBreweriesQuery.HandleAsync(new NotesByBreweryQuery
            {
                BreweryKeys = brewerySearchResults.Breweries.Select(brewery => brewery.NaturalKey).ToArray()
            });

            var brewerySearchViewModel = new BrewerySearchViewModel
            {
                Breweries = brewerySearchResults.Breweries.Select(brewery => new BrewerySearchViewModel.Brewery
                {
                    NaturalKey = brewery.NaturalKey,
                    Name = brewery.Name,
                    StreetAddress = brewery.StreetAddress,
                    Notes = notes.Notes.Where(n => n.Brewery.Equals(brewery.NaturalKey)).Select(n => new BrewerySearchViewModel.Brewery.Note
                    {
                        IsEditable = n.AuthorId.Equals(User?.Identity?.Name),
                        Text = n.Text
                    }).ToArray()
                }).ToArray()
            };
            return View(brewerySearchViewModel);
        }
    }
}