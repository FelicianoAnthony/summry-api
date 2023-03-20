using StarterApi.ApiModels.Query;
using StarterApi.Entities;

namespace StarterApi.Services.Queries
{
    public interface IQueryService
    {
        Query ConvertToEntity(QueryPost postRequest, User user);

        Task<bool> Delete(Query entity);

        Task<List<QueryGet>> GetMany(QueryQueryParams? queryParams);

        Task<QueryGet> GetOne(long id, QueryQueryParams? queryParams);

        Task<Query> GetEntity(long id, QueryQueryParams? queryParams);

        Task<QueryGet> Update(Query existingRow, QueryPatch patchRequest);

        Task<QueryGet> Save(Query newRow);
    }
}
