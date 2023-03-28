using StarterApi.ApiModels.UserSummry;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.UserSummries
{
    public interface IUserSummryRepository : IGenericRepository<UserSummry>
    {
        Task<List<UserSummry>> GetEntities(UserSummryQueryParams? queryParams);

        Task<UserSummry> GetEntity(long id, UserSummryQueryParams? queryParams);
    }
}
