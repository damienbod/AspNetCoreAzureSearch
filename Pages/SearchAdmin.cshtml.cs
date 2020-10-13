using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAzureSearch.Pages
{
    public class SearchAdminModel : PageModel
    {
        private readonly SearchProvider _searchProvider;
        private readonly ILogger<IndexModel> _logger;

        public SearchAdminModel(SearchProvider searchProvider, 
            ILogger<IndexModel> logger)
        {
            _searchProvider = searchProvider;
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public async Task<ActionResult> OnPostCreateIndexAsync()
        {
            await _searchProvider.CreateIndex();
            return Page();
        }

        public async Task<ActionResult> OnPostAddDataAsync()
        {
            PersonCityData.CreateTestData();
            await _searchProvider.AddDocumentsToIndex(PersonCityData.Data);
            return Page();
        }
    }
}
