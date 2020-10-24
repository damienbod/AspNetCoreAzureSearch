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
    public class SearchProviderIndex
    {
        private readonly SearchIndexClient _searchIndexClient;
        private readonly SearchClient _searchClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _index;

        public SearchProviderIndex(IConfiguration configuration, IHttpClientFactory httpClientFactory)
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
                var data = await httpClient.GetAsync(uri).ConfigureAwait(false);
                if (data.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return (false, 0);
                }
                var payload = await data.Content.ReadAsStringAsync().ConfigureAwait(false);
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
    }
}
