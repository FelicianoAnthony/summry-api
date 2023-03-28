using StarterApi.ApiModels.UserSummryQuery;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.UserSummryQueries
{
    public interface IUserSummryQueryRepository : IGenericRepository<UserSummryQuery>
    {
        Task<List<UserSummryQuery>> GetEntities(UserSummryQueryFilters? queryParams);

        Task<UserSummryQuery> GetEntity(long id, UserSummryQueryFilters? queryParams);
    }
}
