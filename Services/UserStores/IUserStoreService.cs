using StarterApi.ApiModels.UserStore;
using StarterApi.Entities;

namespace StarterApi.Services.UserStores
{
    public interface IUserStoreService
    {
        Task<UserStoreGet> Save(User user, Store store);

        Task<List<UserStore>> GetEntities(long userId);

        Task<UserStore> GetEntity(long? userId, long? storeId);

        Task<List<UserStoreGet>> GetMany(long userId);

        Task<UserStoreGet> GetOne(long? userId, long? storeId);

        Task<bool> Delete(UserStore row);

    }
}
