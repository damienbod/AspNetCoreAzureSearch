using BlazorAzureSearch.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAzureSearch.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var model = new SearchData
        {
            SearchText = searchText
        };

        await _searchProvider.QueryPagingFull(model, 0, 0);

        return model;
    }

    [HttpPost]
    [Route("Paging")]
    public async Task<SearchDataDto> Paging([FromBody] SearchDataDto searchDataDto)
    {
        int page;

        switch (searchDataDto.Paging)
        {
            case "prev":
                page = searchDataDto.CurrentPage - 1;
                break;

            case "next":
                page = searchDataDto.CurrentPage + 1;
                break;

            default:
                page = int.Parse(searchDataDto.Paging);
                break;
        }

        int leftMostPage = searchDataDto.LeftMostPage;

        var model = new SearchData
        {
            SearchText = searchDataDto.SearchText,
            LeftMostPage = searchDataDto.LeftMostPage,
            PageCount = searchDataDto.PageCount,
            PageRange = searchDataDto.PageRange,
            Paging = searchDataDto.Paging,
            CurrentPage = searchDataDto.CurrentPage
        };

        await _searchProvider.QueryPagingFull(model, page, leftMostPage);

        var results = new SearchDataDto
        {
            SearchText = model.SearchText,
            LeftMostPage = model.LeftMostPage,
            PageCount = model.PageCount,
            PageRange = model.PageRange,
            Paging = model.Paging,
            CurrentPage = model.CurrentPage,
            Results = new SearchResultItems
            {
                PersonCities = new List<PersonCityDto>(),
                TotalCount = model.PersonCities.TotalCount.GetValueOrDefault()
            }
        };

        var docs = model.PersonCities.GetResults().ToList();
        foreach (var doc in docs)
        {
            results.Results.PersonCities.Add(new PersonCityDto
            {
                CityCountry = doc.Document.CityCountry,
                FamilyName = doc.Document.FamilyName,
                Github = doc.Document.Github,
                Id = doc.Document.Id,
                Info = doc.Document.Info,
                Metadata = doc.Document.Metadata,
                Mvp = doc.Document.Mvp,
                Name = doc.Document.Name,
                Twitter = doc.Document.Twitter,
                Web = doc.Document.Web
            });
        }

        return results;
    }
}
