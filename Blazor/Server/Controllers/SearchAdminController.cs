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

        //[HttpPost]
        //private Task<string> DeleteIndex()
        //{
        //    //wait service.UpdateProductAsync(UpdateProduct)
        //}

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
