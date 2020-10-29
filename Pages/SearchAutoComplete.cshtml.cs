using Azure.Search.Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreAzureSearch.Pages
{
    public class SearchAutoCompleteModel : PageModel
    {
        private readonly SearchProviderAutoComplete _searchProviderAutoComplete;
        private readonly ILogger<IndexModel> _logger;

        public string SearchText { get; set; }

        public SuggestResults<PersonCity> PersonCities;

        public SearchAutoCompleteModel(SearchProviderAutoComplete searchProviderAutoComplete,
            ILogger<IndexModel> logger)
        {
            _searchProviderAutoComplete = searchProviderAutoComplete;
            _logger = logger;
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
}
