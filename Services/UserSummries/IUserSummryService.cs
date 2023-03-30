using SummryApi.ApiModels.UserSummry;
using SummryApi.Entities;

namespace SummryApi.Services.UserSummries
{
    public interface IUserSummryService
    {
        UserSummry ConvertToEntity(UserSummryPost req, User currUser);

        Task Delete(UserSummry row);

        Task<List<UserSummryGet>> GetMany(UserSummryQueryParams? queryParams);

        Task<UserSummryGet> GetOne(long id, UserSummryQueryParams? queryParams);

        Task<UserSummry> GetEntity(long id, UserSummryQueryParams? queryParams);

        Task<List<UserSummry>> GetEntities(UserSummryQueryParams? queryParams);

        Task<UserSummryGet> Save(UserSummry newRow);

        Task<UserSummryGet> Update(UserSummry existingRow, List<UserSummryStore> newStores, List<UserSummryQuery> newQueries);

        UserSummryGet TransformOne(UserSummry row, UserSummryQueryParams? queryParams);

    }
}
