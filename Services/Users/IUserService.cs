using SummryApi.ApiModels.Login;
using SummryApi.ApiModels.User;
using SummryApi.Entities;

namespace SummryApi.Services.Users
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
