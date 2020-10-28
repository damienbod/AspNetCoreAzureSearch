using Azure.Search.Documents.Indexes;

namespace AspNetCoreAzureSearch
{
    public class PersonCity
    {
        [SimpleField(IsFilterable = true, IsKey = true)]
        public string Id { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Name { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string FamilyName { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Info { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string CityCountry { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Metadata { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Web { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Github { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Twitter { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Mvp { get; set; }
    }
}