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

        public async Task AddDocumentsToIndex(List<PersonCity> personCities)
        {
            await UploadDocumentsAsync(_searchClient, personCities);
        }

        private async Task UploadDocumentsAsync(SearchClient searchClient, List<PersonCity> personCities)
        {
            var batch = IndexDocumentsBatch.Upload(personCities);
            try
            {
                await searchClient.IndexDocumentsAsync(batch).ConfigureAwait(false);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Failed to index the documents: \n{0}", ex.Message);
            }
        }
    }
}
