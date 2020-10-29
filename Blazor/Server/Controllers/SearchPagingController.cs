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
    public class SearchPagingController : ControllerBase
    {
        private readonly SearchProviderPaging _searchProvider;
        private readonly ILogger<SearchAdminController> _logger;

        public SearchPagingController(SearchProviderPaging searchProvider,
        ILogger<SearchAdminController> logger)
        {
            _searchProvider = searchProvider;
            _logger = logger;
        }

        [HttpGet]
        public async Task<SearchData> Get(string searchText)
        {
            SearchData model = new SearchData
            {
                SearchText = searchText
            };

            await _searchProvider.QueryPagingFull(model, 0, 0).ConfigureAwait(false);

            return model;
        }

        [HttpPost]
        [Route("Paging")]
        public async Task<SearchData> Paging([FromBody]SearchData model)
        {
            int page;

            switch (model.Paging)
            {
                case "prev":
                    page = model.CurrentPage - 1;
                    break;

                case "next":
                    page = model.CurrentPage + 1;
                    break;

                default:
                    page = int.Parse(model.Paging);
                    break;
            }

            int leftMostPage = model.LeftMostPage;

            await _searchProvider.QueryPagingFull(model, page, leftMostPage).ConfigureAwait(false);

            return model;
        }
    }
}
