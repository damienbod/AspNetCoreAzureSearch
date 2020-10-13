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

        public void CreateIndex()
        {
            var index = new SearchIndex(_index)
            {
                Fields = 
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true, IsFilterable = true, IsSortable = true },
                    new SearchableField("name") { IsFilterable = true, IsSortable = true },
                    new SearchableField("familyName") { IsFilterable = true, IsSortable = true },
                    new SearchableField("info") { IsFilterable = true, IsSortable = true },
                    new SearchableField("cityCountry") { IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new SearchableField("metadata") { IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new SearchableField("web") { IsFilterable = true },
                    new SearchableField("github") { IsFilterable = true },
                    new SearchableField("twitter") { IsFilterable = true },
                    new SearchableField("mvp") { IsFilterable = true},
                }
            };

            _searchIndexClient.CreateIndex(index);
        }

        public async Task AddDocumentsToIndex(List<PersonCity> personCities)
        {
            IndexDocumentsBatch<PersonCity> batch = IndexDocumentsBatch.Create<PersonCity>();

            foreach(var personCity in personCities)
            {
                batch.Actions.Add(IndexDocumentsAction.Upload(personCity));
            }

            IndexDocumentsOptions idxoptions = new IndexDocumentsOptions { ThrowOnAnyError = true };

            await _searchClient.IndexDocumentsAsync(batch, idxoptions);
        }
    }
}
