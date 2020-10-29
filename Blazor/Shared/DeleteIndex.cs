
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorAzureSearch.Shared
{
    public class DeleteIndex
    {
        [JsonPropertyName("messages")]
        public List<AlertViewModel> Messages { get; set; } = new List<AlertViewModel>();
        [JsonPropertyName("status")]
        public IndexStatus Status { get; set; } = new IndexStatus();
    }
}
