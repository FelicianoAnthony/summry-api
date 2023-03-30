using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.UserSummryStores
{
    public interface IUserSummryStoreRepository : IGenericRepository<UserSummryStore>
    {
        Task<List<UserSummryStore>> GetEntities(long userId);

        Task<UserSummryStore> GetEntity(long? userId, long? storeId);
    }
}
