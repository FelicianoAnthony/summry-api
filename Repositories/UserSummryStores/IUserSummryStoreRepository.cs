using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.UserSummryStores
{
    public interface IUserSummryStoreRepository : IGenericRepository<UserSummryStore>
    {
        Task<List<UserSummryStore>> GetEntities(long userId);

        Task<UserSummryStore> GetEntity(long? userId, long? storeId);
    }
}
