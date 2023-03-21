using StarterApi.ApiModels.Login;
using StarterApi.ApiModels.User;
using StarterApi.Entities;

namespace StarterApi.Services.Users
{
    public interface IUserService
    {
        Task<LoginResponse> Authenticate(LoginRequest request);

        Task<long> CreateUser(UserPost req);

        Task<bool> Delete(User store);

        Task<List<UserGet>> GetMany(UserQueryParams? queryParams);

        Task<UserGet> GetOne(long id, UserQueryParams? queryParams);

        Task<User> GetEntity(long id, UserQueryParams? queryParams);

    }
}
