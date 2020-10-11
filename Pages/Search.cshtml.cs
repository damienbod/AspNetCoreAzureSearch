using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAzureSearch.Pages
{
    public class SearchModel : PageModel
    {
        private readonly SearchProvider _searchProvider;
        private readonly ILogger<IndexModel> _logger;

        public SearchModel(SearchProvider searchProvider,
            ILogger<IndexModel> logger)
        {
            _searchProvider = searchProvider;
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
