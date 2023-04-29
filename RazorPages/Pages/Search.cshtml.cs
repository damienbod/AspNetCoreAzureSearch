using Azure.Search.Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreAzureSearch.Pages;

public class SearchModel : PageModel
{
    private readonly SearchProviderPaging? _searchProvider;

    public string? SearchText { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int LeftMostPage { get; set; }
    public int PageRange { get; set; }
    public string? Paging { get; set; }
    public int PageNo { get; set; }
    public SearchResults<PersonCity>? PersonCities;

    public SearchModel(SearchProviderPaging searchProvider)
    {
        _searchProvider = searchProvider;
    }

    public void OnGet()
    {
    }

    public async Task<ActionResult> OnGetInitAsync(string searchText)
    {
        var model = new SearchData
        {
            SearchText = searchText
        };

        await _searchProvider.QueryPagingFull(model, 0, 0).ConfigureAwait(false);

        SearchText = model.SearchText;
        CurrentPage = model.CurrentPage;
        PageCount = model.PageCount;
        LeftMostPage = model.LeftMostPage;
        PageRange = model.PageRange;
        Paging = model.Paging;
        PersonCities = model.PersonCities;

        return Page();
    }

    public async Task<ActionResult> OnGetPagingAsync(SearchData model)
    {
        int page;

        switch (model.Paging)
        {
            case "prev":
                page = PageNo - 1;
                break;

            case "next":
                page = PageNo + 1;
                break;

            default:
                page = int.Parse(model.Paging);
                break;
        }

        int leftMostPage = LeftMostPage;

        await _searchProvider.QueryPagingFull(model, page, leftMostPage).ConfigureAwait(false);

        PageNo = page;
        SearchText = model.SearchText;
        CurrentPage = model.CurrentPage;
        PageCount = model.PageCount;
        LeftMostPage = model.LeftMostPage;
        PageRange = model.PageRange;
        Paging = model.Paging;
        PersonCities = model.PersonCities;

        return Page();
    }

}
