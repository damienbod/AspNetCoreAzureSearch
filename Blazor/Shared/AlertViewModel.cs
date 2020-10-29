
using System.Text.Json.Serialization;

namespace BlazorAzureSearch.Shared
{
    public class AlertViewModel
    {
        [JsonPropertyName("alertType")]
        public string AlertType { get; set; }
        [JsonPropertyName("alertTitle")]
        public string AlertTitle { get; set; }
        [JsonPropertyName("alertMessage")]
        public string AlertMessage { get; set; }

        public AlertViewModel() { }

        public AlertViewModel(string type, string title, string message)
        {
            AlertType = type;
            AlertTitle = title;
            AlertMessage = message;
        }
    }
}
