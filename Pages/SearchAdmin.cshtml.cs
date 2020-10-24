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
        private readonly SearchProviderIndex _searchProviderIndex;
        private readonly ILogger<IndexModel> _logger;

        public AlertViewModel[] Messages = null;
        public SearchAdminModel(SearchProviderIndex searchProviderIndex,
            ILogger<IndexModel> logger)
        {
            _searchProviderIndex = searchProviderIndex;
            _logger = logger;
        }

        public string IndexName { get; set; } = "personcities";
        public bool IndexExists { get; set; }
        public long DocumentCount { get; set; }

        public async Task OnGetAsync()
        {
            var indexStatus = await _searchProviderIndex.GetIndexStatus();
            IndexExists = indexStatus.Exists;
            DocumentCount = indexStatus.DocumentCount;
        }

        public async Task<ActionResult> OnPostCreateIndexAsync()
        {
            try
            {
                await _searchProviderIndex.CreateIndex();
                Messages = new[] {
                    new AlertViewModel("success", "Index created", "The Azure Search index was created successfully!"),
                };
                var indexStatus = await _searchProviderIndex.GetIndexStatus();
                IndexExists = indexStatus.Exists;
                DocumentCount = indexStatus.DocumentCount;
                return Page();
            }
            catch (Exception ex)
            {
                Messages = new[] {
                    new AlertViewModel("danger", "Error creating index", ex.Message),
                };
                return Page();
            }
        }

        public async Task<ActionResult> OnPostAddDataAsync()
        {
            try
            {
                PersonCityData.CreateTestData();
                await _searchProviderIndex.AddDocumentsToIndex(PersonCityData.Data);
                Messages = new[] {
                    new AlertViewModel("success", "Documented added", "The Azure Search documents were uploaded! The Document Count takes n seconds to update!"),
                };
                var indexStatus = await _searchProviderIndex.GetIndexStatus();
                IndexExists = indexStatus.Exists;
                DocumentCount = indexStatus.DocumentCount;
                return Page();
            }
            catch (Exception ex)
            {
                Messages = new[] {
                    new AlertViewModel("danger", "Error adding documents", ex.Message),
                };
                return Page();
            }
        }

        public async Task<ActionResult> OnPostDeleteIndexAsync()
        {
            try
            {
                await _searchProviderIndex.DeleteIndex();
                Messages = new[] {
                    new AlertViewModel("success", "Index Deleted!", "The Azure Search Index was successfully deleted!"),
                };
                var indexStatus = await _searchProviderIndex.GetIndexStatus();
                IndexExists = indexStatus.Exists;
                DocumentCount = indexStatus.DocumentCount;
                return Page();
            }
            catch (Exception ex)
            {
                Messages = new[] {
                    new AlertViewModel("danger", "Error deleting index", ex.Message),
                };
                return Page();
            }
        }

    }
}
