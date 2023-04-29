using System.Text.Json.Serialization;

namespace BlazorAzureSearch.Shared;

public class PersonCityDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("familyName")]
    public string? FamilyName { get; set; }

    [JsonPropertyName("info")]
    public string? Info { get; set; }

    [JsonPropertyName("cityCountry")]
    public string? CityCountry { get; set; }

    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }

    [JsonPropertyName("web")]
    public string? Web { get; set; }

    [JsonPropertyName("github")]
    public string? Github { get; set; }

    [JsonPropertyName("twitter")]
    public string? Twitter { get; set; }

    [JsonPropertyName("mvp")]
    public string? Mvp { get; set; }
}