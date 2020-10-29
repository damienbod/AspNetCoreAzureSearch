
using System.Text.Json.Serialization;

namespace BlazorAzureSearch.Shared
{
    public class IndexStatus
    {
        [JsonPropertyName("indexExists")]
        public bool IndexExists { get; set; }
        [JsonPropertyName("documentCount")]
        public long DocumentCount { get; set; }
    }
}
