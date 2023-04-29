using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace BlazorAzureSearch.Server;

public class SearchProviderPaging
{
    private readonly SearchClient? _searchClient;
    private readonly string? _index;

    public SearchProviderPaging(IConfiguration configuration)
    {
        _index = configuration["PersonCitiesIndexName"];

        Uri serviceEndpoint = new Uri(configuration["PersonCitiesSearchUri"]);
        AzureKeyCredential credential = new AzureKeyCredential(configuration["PersonCitiesSearchApiKey"]);
        _searchClient = new SearchClient(serviceEndpoint, _index, credential);
    }

    public async Task QueryPagingFull(SearchData model, int page, int leftMostPage)
    {
        var pageSize = 4;
        var maxPageRange = 7;
        var pageRangeDelta = maxPageRange - pageSize;

        var options = new SearchOptions
        {
            Skip = page * pageSize,
            Size = pageSize,
            IncludeTotalCount = true,
            QueryType = SearchQueryType.Full
        }; // options.Select.Add("Name"); // add this explicitly if all fields are not required

        model.PersonCities = await _searchClient.SearchAsync<PersonCity>(model.SearchText, options).ConfigureAwait(false);
        model.PageCount = ((int)model.PersonCities.TotalCount + pageSize - 1) / pageSize;
        model.CurrentPage = page;
        if (page == 0)
        {
            leftMostPage = 0;
        }
        else if (page <= leftMostPage)
        {
            leftMostPage = Math.Max(page - pageRangeDelta, 0);
        }
        else if (page >= leftMostPage + maxPageRange - 1)
        {
            leftMostPage = Math.Min(page - pageRangeDelta, model.PageCount - maxPageRange);
        }
        model.LeftMostPage = leftMostPage;
        model.PageRange = Math.Min(model.PageCount - leftMostPage, maxPageRange);
    }
}
