
namespace BlazorAzureSearch.Shared
{
    public class DeleteIndex
    {
        public AlertViewModel[] Messages { get; set; }
        public IndexStatus Status { get; set; } = new IndexStatus();
    }
}
