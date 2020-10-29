using Azure.Search.Documents.Indexes.Models;
using BlazorAzureSearch.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAzureSearch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchAdminController : ControllerBase
    {
        private readonly SearchProviderIndex _searchProviderIndex;
        private readonly ILogger<SearchAdminController> _logger;

        public SearchAdminController(SearchProviderIndex searchProviderIndex,
            ILogger<SearchAdminController> logger)
        {
            _searchProviderIndex = searchProviderIndex;
            _logger = logger;
        }

        [HttpGet]
        [Route("IndexStatus")]
        public async Task<IndexStatus> IndexStatus()
        {
            var indexStatus = await _searchProviderIndex.GetIndexStatus().ConfigureAwait(false);
            return new IndexStatus
            {
                IndexExists = indexStatus.Exists,
                DocumentCount = indexStatus.DocumentCount
            };
        }

        [HttpPost]
        [Route("DeleteIndex")]
        public async Task<DeleteIndex> DeleteIndex(string indexName)
        {
            var deleteIndex = new DeleteIndex();
            if (string.IsNullOrEmpty(indexName))
            {
                deleteIndex.Messages = new[] {
                    new AlertViewModel("danger", "no indexName defined", "Please provide the index name"),
                };
                return deleteIndex;
            }

            try
            {
                await _searchProviderIndex.DeleteIndex(indexName).ConfigureAwait(false);

                deleteIndex.Messages = new[] {
                    new AlertViewModel("success", "Index Deleted!", "The Azure Search Index was successfully deleted!"),
                };
                var indexStatus = await _searchProviderIndex.GetIndexStatus().ConfigureAwait(false);
                deleteIndex.Status.IndexExists = indexStatus.Exists;
                deleteIndex.Status.DocumentCount = indexStatus.DocumentCount;
                return deleteIndex;
            }
            catch (Exception ex)
            {
                deleteIndex.Messages = new[] {
                    new AlertViewModel("danger", "Error deleting index", ex.Message),
                };
                return deleteIndex;
            }
        }

        //[HttpPost]
        //private void AddData()
        //{
        //    //wait service.UpdateProductAsync(UpdateProduct)
        //}

        //[HttpPost]
        //private void CreateIndex()
        //{
        //    //wait service.UpdateProductAsync(UpdateProduct)
        //}
    }
}
