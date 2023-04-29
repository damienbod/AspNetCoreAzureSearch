using System.Text.Json.Serialization;

namespace BlazorAzureSearch.Shared;

public class SearchResultItems
{
    // The text to search for.
    [JsonPropertyName("totalCount")]
    public long TotalCount { get; set; }

    [JsonPropertyName("personCities")]
    public List<PersonCityDto>? PersonCities { get; set; }
}