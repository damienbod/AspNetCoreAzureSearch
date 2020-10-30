using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using BlazorAzureSearch.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BlazorAzureSearch.Server
{
    public class SearchProviderAutoComplete
    {
        private readonly SearchClient _searchClient;
        private readonly string _index;

        public SearchProviderAutoComplete(IConfiguration configuration)
        {
            _index = configuration["PersonCitiesIndexName"];

            Uri serviceEndpoint = new Uri(configuration["PersonCitiesSearchUri"]);
            AzureKeyCredential credential = new AzureKeyCredential(configuration["PersonCitiesSearchApiKey"]);
            _searchClient = new SearchClient(serviceEndpoint, _index, credential);

        }

        public async Task<SuggestResults<PersonCity>> Suggest(
            bool highlights, bool fuzzy, string term)
        {
            SuggestOptions sp = new SuggestOptions()
            {
                UseFuzzyMatching = fuzzy,
                Size = 5,
            };
            sp.Select.Add("Id");
            sp.Select.Add("Name");
            sp.Select.Add("FamilyName");
            sp.Select.Add("Info");
            sp.Select.Add("CityCountry");
            sp.Select.Add("Web");

            if (highlights)
            {
                sp.HighlightPreTag = "<b>";
                sp.HighlightPostTag = "</b>";
            }

            var suggestResults = await _searchClient.SuggestAsync<PersonCity>(term, "personSg", sp).ConfigureAwait(false);
            return suggestResults.Value;
        }

        //public async Task<List<string>> AutoComplete(string term)
        //{
        //    AutocompleteOptions ap = new AutocompleteOptions()
        //    {
        //        UseFuzzyMatching = false, Size = 5, 
        //    };

        //    var autocompleteResult = await _searchClient.AutocompleteAsync(term, "personSg", ap).ConfigureAwait(false);

        //    List<string> autocomplete = autocompleteResult.Value.Results.Select(x => x.Text).ToList();
        //    return autocomplete;
        //}
    }
}
