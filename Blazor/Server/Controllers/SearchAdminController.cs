using BlazorAzureSearch.Server.PersonSearch;
using BlazorAzureSearch.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAzureSearch.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchAdminController : ControllerBase
{
    private readonly SearchProviderIndex _searchProviderIndex;

    public SearchAdminController(SearchProviderIndex searchProviderIndex)
    {
        _searchProviderIndex = searchProviderIndex;
    }

    [HttpGet]
    [Route("IndexStatus")]
    public async Task<IndexStatus> IndexStatus()
    {
        var indexStatus = await _searchProviderIndex.GetIndexStatus();
        return new IndexStatus
        {
            IndexExists = indexStatus.Exists,
            DocumentCount = indexStatus.DocumentCount
        };
    }

    [HttpPost]
    [Route("DeleteIndex")]
    public async Task<IndexResult> DeleteIndex([FromBody] string indexName)
    {
        var deleteIndex = new IndexResult();
        if (string.IsNullOrEmpty(indexName))
        {
            deleteIndex.Messages = [
                new AlertViewModel("danger", "no indexName defined", "Please provide the index name"),
            ];
            return deleteIndex;
        }

        try
        {
            await _searchProviderIndex.DeleteIndex(indexName);

            deleteIndex.Messages = [
                new AlertViewModel("success", "Index Deleted!", "The Azure Search Index was successfully deleted!"),
            ];
            var indexStatus = await _searchProviderIndex.GetIndexStatus();
            deleteIndex.Status.IndexExists = indexStatus.Exists;
            deleteIndex.Status.DocumentCount = indexStatus.DocumentCount;
            return deleteIndex;
        }
        catch (Exception ex)
        {
            deleteIndex.Messages = [
                new AlertViewModel("danger", "Error deleting index", ex.Message),
            ];

            return deleteIndex;
        }
    }

    [HttpPost]
    [Route("AddData")]
    public async Task<IndexResult> AddData([FromBody] string indexName)
    {
        var addData = new IndexResult();
        if (string.IsNullOrEmpty(indexName))
        {
            addData.Messages = [
                new AlertViewModel("danger", "no indexName defined", "Please provide the index name"),
            ];
            return addData;
        }
        try
        {
            PersonCityData.CreateTestData();
            await _searchProviderIndex.AddDocumentsToIndex(PersonCityData.Data);
            addData.Messages = [
                new AlertViewModel("success", "Documented added", "The Azure Search documents were uploaded! The Document Count takes n seconds to update!"),
            ];
            var indexStatus = await _searchProviderIndex.GetIndexStatus();
            addData.Status.IndexExists = indexStatus.Exists;
            addData.Status.DocumentCount = indexStatus.DocumentCount;
            return addData;
        }
        catch (Exception ex)
        {
            addData.Messages = [
                new AlertViewModel("danger", "Error adding documents", ex.Message),
            ];
            return addData;
        }
    }

    [HttpPost]
    [Route("CreateIndex")]
    public async Task<IndexResult> CreateIndex([FromBody] string indexName)
    {
        var createIndex = new IndexResult();
        if (string.IsNullOrEmpty(indexName))
        {
            createIndex.Messages = [
                new AlertViewModel("danger", "no indexName defined", "Please provide the index name"),
            ];
            return createIndex;
        }

        try
        {
            await _searchProviderIndex.CreateIndex();
            createIndex.Messages = [
                new AlertViewModel("success", "Index created", "The Azure Search index was created successfully!"),
            ];
            var indexStatus = await _searchProviderIndex.GetIndexStatus();
            createIndex.Status.IndexExists = indexStatus.Exists;
            createIndex.Status.DocumentCount = indexStatus.DocumentCount;
            return createIndex;
        }
        catch (Exception ex)
        {
            createIndex.Messages = [
                new AlertViewModel("danger", "Error creating index", ex.Message),
            ];
            return createIndex;
        }

    }
}
