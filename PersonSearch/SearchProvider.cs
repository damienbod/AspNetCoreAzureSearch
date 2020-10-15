using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAzureSearch
{
    public class SearchProvider
    {
        private readonly SearchIndexClient _searchIndexClient;
        private readonly SearchClient _searchClient;
        private readonly string _index;

        public SearchProvider(IConfiguration configuration)
        {
            Uri serviceEndpoint = new Uri(configuration["PersonCitiesSearchUri"]);
            AzureKeyCredential credential = new AzureKeyCredential(configuration["PersonCitiesSearchApiKey"]);
            _searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);
            _index = configuration["PersonCitiesIndexName"];
            _searchClient = new SearchClient(serviceEndpoint, _index, credential);
        }

        public async Task CreateIndex()
        {
            FieldBuilder bulder = new FieldBuilder();
            var definition = new SearchIndex(_index, bulder.Build(typeof(PersonCity)));

            await _searchIndexClient.CreateIndexAsync(definition).ConfigureAwait(false);
        }

        public async Task DeleteIndex()
        {
            await _searchIndexClient.DeleteIndexAsync(_index).ConfigureAwait(false);
        }

        public async Task AddDocumentsToIndex(List<PersonCity> personCities)
        {
            var batch = IndexDocumentsBatch.Upload(personCities);
            await _searchClient.IndexDocumentsAsync(batch).ConfigureAwait(false);
        }

        public async Task RunQueryAsync(SearchData model, int page, int leftMostPage)
        {
            var pageSize = 4;
            var maxPageRange = 7;
            var pageRangeDelta = 3;

            var options = new SearchOptions
            {
                Skip = page * pageSize,
                Size = pageSize,
                IncludeTotalCount = true
            };

            // options.Select.Add("Name");
            // options.Select.Add("CityCountry");

            // For efficiency, the search call should be asynchronous, so use SearchAsync rather than Search.
            model.PersonCities = await _searchClient.SearchAsync<PersonCity>(model.SearchText, options).ConfigureAwait(false);

            // This variable communicates the total number of pages to the view.
            model.PageCount = ((int)model.PersonCities.TotalCount + pageSize - 1) / pageSize;

            // This variable communicates the page number being displayed to the view.
            model.CurrentPage = page;

            // Calculate the range of page numbers to display.
            if (page == 0)
            {
                leftMostPage = 0;
            }
            else if (page <= leftMostPage)
            {
                // Trigger a switch to a lower page range.
                leftMostPage = Math.Max(page - pageRangeDelta, 0);
            }
            else if (page >= leftMostPage + maxPageRange - 1)
            {
                // Trigger a switch to a higher page range.
                leftMostPage = Math.Min(page - pageRangeDelta, model.PageCount - maxPageRange);
            }
            model.LeftMostPage = leftMostPage;

            // Calculate the number of page numbers to display.
            model.PageRange = Math.Min(model.PageCount - leftMostPage, maxPageRange);

        }
    }
}
