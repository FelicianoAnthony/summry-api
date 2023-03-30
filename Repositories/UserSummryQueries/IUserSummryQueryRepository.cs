using SummryApi.ApiModels.UserSummryQuery;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.UserSummryQueries
{
    public interface IUserSummryQueryRepository : IGenericRepository<UserSummryQuery>
    {
        Task<List<UserSummryQuery>> GetEntities(UserSummryQueryFilters? queryParams);

        Task<UserSummryQuery> GetEntity(long id, UserSummryQueryFilters? queryParams);
    }
}
