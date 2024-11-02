using Azure.Search.Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreAzureAISearch.Pages;

public class SearchAutoCompleteModel : PageModel
{
    private readonly SearchProviderAutoComplete _searchProviderAutoComplete;

    public string? SearchText { get; set; }

    public SuggestResults<PersonCity>? PersonCities;

    public SearchAutoCompleteModel(SearchProviderAutoComplete searchProviderAutoComplete)
    {
        _searchProviderAutoComplete = searchProviderAutoComplete;
    }

    public void OnGet()
    {
    }

    public async Task<ActionResult> OnGetAutoCompleteSuggest(string term)
    {
        //List<string> suggestions = PersonCities.Results.Select(x => x.Text).ToList();
        PersonCities = await _searchProviderAutoComplete.Suggest(false, true, term);
        SearchText = term;

        return new JsonResult(PersonCities.Results);
    }

    //public async Task OnGetAutoComplete(string term)
    //{
    //    var data = await _searchProviderAutoComplete.AutoComplete(term);
    //}
}
