using StarterApi.ApiModels.Store;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.Stores
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<List<Store>> GetEntities(StoreQueryParams? queryParams);

        Task<Store> GetEntity(long id, StoreQueryParams? queryParams);

        Task<Store> FindOneByParams(StoreQueryParams? queryParams); 
    }
}
