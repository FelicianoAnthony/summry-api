using StarterApi.ApiModels.Query;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.Queries
{
    public interface IQueryRepository : IGenericRepository<Query>
    {
        Task<List<Query>> GetEntities(QueryQueryParams? queryParams);

        Task<Query> GetEntity(long id, QueryQueryParams? queryParams);
    }
}
