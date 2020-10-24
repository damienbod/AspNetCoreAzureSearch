using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AspNetCoreAzureSearch
{
    public class SearchProvider
    {
        private readonly SearchIndexClient _searchIndexClient;
        private readonly SearchClient _searchClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _index;

        public SearchProvider(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _index = configuration["PersonCitiesIndexName"];

            Uri serviceEndpoint = new Uri(configuration["PersonCitiesSearchUri"]);
            AzureKeyCredential credential = new AzureKeyCredential(configuration["PersonCitiesSearchApiKey"]);

            _searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);
            _searchClient = new SearchClient(serviceEndpoint, _index, credential);

        }

        public async Task CreateIndex()
        {
            FieldBuilder bulder = new FieldBuilder();
            var definition = new SearchIndex(_index, bulder.Build(typeof(PersonCity)));
            definition.Suggesters.Add(new SearchSuggester(
                "person", new string[] { "name", "familyName", "info", "cityCountry" }
            ));

            await _searchIndexClient.CreateIndexAsync(definition).ConfigureAwait(false);
        }

        public async Task DeleteIndex()
        {
            await _searchIndexClient.DeleteIndexAsync(_index).ConfigureAwait(false);
        }

        public async Task<(bool Exists, long DocumentCount)> GetIndexStatus()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                };
                httpClient.DefaultRequestHeaders.Add("api-key", _configuration["PersonCitiesSearchApiKey"]);

                var uri = $"{_configuration["PersonCitiesSearchUri"]}/indexes/{_index}/docs/$count?api-version=2020-06-30";
                var data = await httpClient.GetAsync(uri);
                if (data.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (false, 0);
                }
                var payload = await data.Content.ReadAsStringAsync();
                return (true, int.Parse(payload));
            }
            catch
            {
                return (false, 0);
            }
        }

        public async Task AddDocumentsToIndex(List<PersonCity> personCities)
        {
            var batch = IndexDocumentsBatch.Upload(personCities);
            await _searchClient.IndexDocumentsAsync(batch).ConfigureAwait(false);
        }

        public async Task QueryPagingFull(SearchData model, int page, int leftMostPage)
        {
            var pageSize = 4;
            var maxPageRange = 7;
            var pageRangeDelta = maxPageRange - pageSize;

            var options = new SearchOptions
            {
                Skip = page * pageSize,
                Size = pageSize,
                IncludeTotalCount = true,
                QueryType = SearchQueryType.Full
            }; // options.Select.Add("Name"); // add this explicitly if all fields are not required

            model.PersonCities = await _searchClient.SearchAsync<PersonCity>(model.SearchText, options).ConfigureAwait(false);
            model.PageCount = ((int)model.PersonCities.TotalCount + pageSize - 1) / pageSize;
            model.CurrentPage = page;
            if (page == 0)
            {
                leftMostPage = 0;
            }
            else if (page <= leftMostPage)
            {
                leftMostPage = Math.Max(page - pageRangeDelta, 0);
            }
            else if (page >= leftMostPage + maxPageRange - 1)
            {
                leftMostPage = Math.Min(page - pageRangeDelta, model.PageCount - maxPageRange);
            }
            model.LeftMostPage = leftMostPage;
            model.PageRange = Math.Min(model.PageCount - leftMostPage, maxPageRange);
        }
    }
}
