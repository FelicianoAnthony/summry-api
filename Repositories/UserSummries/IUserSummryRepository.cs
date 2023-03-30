using SummryApi.ApiModels.UserSummry;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.UserSummries
{
    public interface IUserSummryRepository : IGenericRepository<UserSummry>
    {
        Task<List<UserSummry>> GetEntities(UserSummryQueryParams? queryParams);

        Task<UserSummry> GetEntity(long id, UserSummryQueryParams? queryParams);
    }
}
