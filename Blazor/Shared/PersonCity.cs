using Azure.Search.Documents.Indexes;
using System.Text.Json.Serialization;

namespace BlazorAzureSearch.Shared
{
    public class PersonCity
    {
        [SimpleField(IsFilterable = true, IsKey = true)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        [JsonPropertyName("familyName")]
        public string FamilyName { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        [JsonPropertyName("info")]
        public string Info { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        [JsonPropertyName("cityCountry")]
        public string CityCountry { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        [JsonPropertyName("metadata")]
        public string Metadata { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        [JsonPropertyName("web")]
        public string Web { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        [JsonPropertyName("github")]
        public string Github { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        [JsonPropertyName("twitter")]
        public string Twitter { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        [JsonPropertyName("mvp")]
        public string Mvp { get; set; }
    }
}