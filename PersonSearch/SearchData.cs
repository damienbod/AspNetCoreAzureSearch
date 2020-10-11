using Azure.Search.Documents.Models;

namespace AspNetCoreAzureSearch
{
    public class SearchData
    {
        public string SearchText { get; set; }

        public string Paging { get; set; }

        public SearchResults<PersonCity> PersonCities;

        //public IEnumerable<PersonCity> PersonCities { get; set; }
    }
}