using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorAzureSearch.Shared
{
    public class SearchDataDto
    {
        // The text to search for.
        [JsonPropertyName("searchText")]
        public string SearchText { get; set; }

        // The current page being displayed.
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        // The total number of pages of results.
        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; }

        // The left-most page number to display.
        [JsonPropertyName("leftMostPage")]
        public int LeftMostPage { get; set; }

        // The number of page numbers to display - which can be less than MaxPageRange towards the end of the results.
        [JsonPropertyName("pageRange")]
        public int PageRange { get; set; }

        // Used when page numbers, or next or prev buttons, have been selected.
        [JsonPropertyName("paging")]
        public string Paging { get; set; } = "0";

        [JsonPropertyName("results")]
        public SearchResultItems Results { get; set; } = new SearchResultItems
        {
            PersonCities = new List<PersonCityDto>()
        };
    }
}