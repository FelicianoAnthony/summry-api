using StarterApi.ApiModels.UserSummryQuery;
using StarterApi.Entities;

namespace StarterApi.Services.UserSummryQueryService
{
    public interface IUserSummryQueryService
    {
        List<UserSummryQuery> ConvertToEntities(List<UserSummryQueryPost> requestUserQueries, UserSummry userSummry);

        Task<bool> Delete(UserSummryQuery entity);

        Task<List<UserSummryQueryGet>> GetMany(UserSummryQueryFilters? queryParams);

        Task<UserSummryQueryGet> GetOne(long id, UserSummryQueryFilters? queryParams);

        Task<UserSummryQuery> GetEntity(long id, UserSummryQueryFilters? queryParams);

        Task<UserSummryQueryGet> Update(UserSummryQuery existingRow, UserSummryQueryPatch patchRequest);

        Task<UserSummryQueryGet> Save(UserSummryQuery newRow);
    }
}
