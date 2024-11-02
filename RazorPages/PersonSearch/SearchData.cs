using Azure.Search.Documents.Models;

namespace AspNetCoreAzureAISearch;

public class SearchData
{
    // The text to search for.
    public string? SearchText { get; set; }

    // The current page being displayed.
    public int CurrentPage { get; set; }

    // The total number of pages of results.
    public int PageCount { get; set; }

    // The left-most page number to display.
    public int LeftMostPage { get; set; }

    // The number of page numbers to display - which can be less than MaxPageRange towards the end of the results.
    public int PageRange { get; set; }

    // Used when page numbers, or next or prev buttons, have been selected.
    public string? Paging { get; set; }

    public SearchResults<PersonCity>? PersonCities { get; set; }

}