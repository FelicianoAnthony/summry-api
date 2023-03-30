using SummryApi.ApiModels.User;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> FindUserByProps(UserQueryParams queryParams);

        Task<List<User>> GetEntities(UserQueryParams? queryParams);

        Task<User> GetEntity(long id, UserQueryParams? queryParams);
    }
}
