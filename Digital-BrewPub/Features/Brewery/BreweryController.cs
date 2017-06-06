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

        public async Task<IActionResult> Search()
        {
            var brewerySearchResults = await searchHandler.HandleAsync(new BrewerySearchRequest());
            var notes = await notesForBreweriesQuery.HandleAsync(new NotesByBreweryQuery
            {
                BreweryNames = brewerySearchResults.Breweries.Select(brewery => brewery.Name).ToArray()
            });

            var brewerySearchViewModel = new BrewerySearchViewModel
            {
                Breweries = brewerySearchResults.Breweries.Select(brewery => new BrewerySearchViewModel.Brewery
                {
                    Name = brewery.Name,
                    StreetAddress = brewery.StreetAddress,
                    Notes = notes.Notes.Where(n => n.Brewery.Equals(brewery.Name)).Select(n => new BrewerySearchViewModel.Brewery.Note
                    {
                        IsEditable = false,
                        Text = n.Text
                    }).ToArray()
                }).ToArray()
            };
            return View(brewerySearchViewModel);
        }
    }
}