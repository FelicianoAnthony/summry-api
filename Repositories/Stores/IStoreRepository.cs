using SummryApi.ApiModels.Store;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.Stores
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<List<Store>> GetEntities(StoreQueryParams? queryParams);

        Task<Store> GetEntity(long id, StoreQueryParams? queryParams);

        Task<Store> FindOneByParams(StoreQueryParams? queryParams); 
    }
}
