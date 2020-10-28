using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using System.Text.Json.Serialization;

namespace AspNetCoreAzureSearch
{
    public class PersonCityMappingDto
    {
        public long Id { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
        public string Name { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
        public string FamilyName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
        public string Info { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
        public string CityCountry { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
        public string Metadata { get; set; }

        public string Web { get; set; }

        public string Github { get; set; }

        public string Twitter { get; set; }

        public string Mvp { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
        public string searchfield { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
        public string autocomplete { get; set; }
    }
}
