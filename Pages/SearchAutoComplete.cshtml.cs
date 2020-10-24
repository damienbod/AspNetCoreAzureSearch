using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AspNetCoreAzureSearch.Pages
{
    public class SearchAutoCompleteModel : PageModel
    {
        private readonly SearchProviderPaging _searchProvider;
        private readonly ILogger<IndexModel> _logger;

        public string SearchText { get; set; }

        public SearchAutoCompleteModel(SearchProviderPaging searchProvider,
            ILogger<IndexModel> logger)
        {
            _searchProvider = searchProvider;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnGetAutoComplete(string term)
        {
            var zz = term;
        }

        

    }
}
