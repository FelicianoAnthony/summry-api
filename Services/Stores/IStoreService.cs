using StarterApi.ApiModels.Store;
using StarterApi.Entities;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Stores
{
    public interface IStoreService : IBaseService<Store, StoreGet, StorePost, StorePatch, StoreQueryParams>
    {

        Task<Store> FindOrShouldCreate(StorePost req);
    }
}
