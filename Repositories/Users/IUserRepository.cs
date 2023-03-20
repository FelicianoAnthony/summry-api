using StarterApi.ApiModels.User;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> FindUserByProps(UserQueryParams queryParams);

        Task<List<User>> GetEntities(UserQueryParams? queryParams);

        Task<User> GetEntity(long id, UserQueryParams? queryParams);
    }
}
