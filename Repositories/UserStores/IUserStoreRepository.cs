using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.UserStores
{
    public interface IUserStoreRepository : IGenericRepository<UserStore>
    {
        Task<List<UserStore>> GetEntities(long userId);

        Task<UserStore> GetEntity(long? userId, long? storeId);
    }
}
