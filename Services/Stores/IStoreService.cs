using StarterApi.ApiModels.Store;
using StarterApi.Entities;

namespace StarterApi.Services.Stores
{
    public interface IStoreService
    {
        Store ConvertToEntity(StorePost req, Platform platform);

        Task<List<UserSummryStore>> ConvertToEntities(List<StorePost> requestUserStores, UserSummry userSummry);

        Task<Platform> CheckUrlScrapability(string url);

        Task<bool> Delete(Store store);

        Task<List<StoreGet>> GetMany(StoreQueryParams? queryParams);

        Task<StoreGet> GetOne(long id, StoreQueryParams? queryParams);

        Task<Store> GetEntity(long id, StoreQueryParams? queryParams);

        Task<StoreGet> Save(Store newRow);

        Task<StoreGet> Update(Store existingRow, StorePatch req);

        StoreGet TransformOne(Store row, StoreQueryParams? queryParams);

    }
}
