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

        public AlertViewModel[] Messages = null;
        public SearchAdminModel(SearchProvider searchProvider,
            ILogger<IndexModel> logger)
        {
            _searchProvider = searchProvider;
            _logger = logger;
        }

        public void OnGet()
        {
            //Messages = new[] {
            //    new AlertViewModel("success", "Success!", "The object was added successfully!"),
            //    new AlertViewModel("warning", "Warning!", "The object was added with a warning!"),
            //    new AlertViewModel("danger", "Danger!", "The object was not added!")
            //};
        }

        public async Task<ActionResult> OnPostCreateIndexAsync()
        {
            try
            {
                await _searchProvider.CreateIndex();
                Messages = new[] {
                    new AlertViewModel("success", "Index created", "The Azure Search index was created successfully!"),
                };
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
                await _searchProvider.AddDocumentsToIndex(PersonCityData.Data);
                Messages = new[] {
                    new AlertViewModel("success", "Documented added", "The Azure Search documents were uploaded!"),
                };
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
                await _searchProvider.DeleteIndex();
                Messages = new[] {
                    new AlertViewModel("success", "Index Deleted!", "The Azure Search Index was successfully deleted!"),
                };
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
