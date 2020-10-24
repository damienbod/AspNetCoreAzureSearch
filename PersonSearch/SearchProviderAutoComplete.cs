using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AspNetCoreAzureSearch
{
    public class SearchProviderAutoComplete
    {
        private readonly SearchIndexClient _searchIndexClient;
        private readonly SearchClient _searchClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _index;

        public SearchProviderAutoComplete(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _index = configuration["PersonCitiesIndexName"];

            Uri serviceEndpoint = new Uri(configuration["PersonCitiesSearchUri"]);
            AzureKeyCredential credential = new AzureKeyCredential(configuration["PersonCitiesSearchApiKey"]);

            _searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);
            _searchClient = new SearchClient(serviceEndpoint, _index, credential);

        }

        public async Task<List<string>> Suggest(bool highlights, bool fuzzy, string term)
        {
            SuggestOptions sp = new SuggestOptions()
            {
                UseFuzzyMatching = fuzzy, 
                Size = 5
            };

            if (highlights)
            {
                sp.HighlightPreTag = "<b>";
                sp.HighlightPostTag = "</b>";
            }

            var resp = await _searchClient.SuggestAsync<PersonCity>(term, "personSg", sp).ConfigureAwait(false);

            List<string> suggestions = resp.Value.Results.Select(x => x.Text).ToList();
            return suggestions;
        }

        public async Task<List<string>> AutoComplete(string term)
        {
            AutocompleteOptions ap = new AutocompleteOptions()
            {
                UseFuzzyMatching = false, Size = 5
            };

            var autocompleteResult = await _searchClient.AutocompleteAsync(term, "personSg", ap).ConfigureAwait(false);

            List<string> autocomplete = autocompleteResult.Value.Results.Select(x => x.Text).ToList();
            return autocomplete;
        }
    }
}
