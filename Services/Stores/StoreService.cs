using StarterApi.ApiModels.Platform;
using StarterApi.ApiModels.Product;
using StarterApi.ApiModels.Store;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Stores
{
    public class StoreService : IBaseService<Store, StoreGet, StorePost, StorePatch, StoreQueryParams>, IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Store ConvertToEntity(StorePost req)
        {
            return new Store
            { 
                Name = req.Name,
                Url = req.Url,
                Description = req.Description
            };
        }

        public async Task<bool> Delete(Store store)
        {
            // TODO: change way delete works...
            _unitOfWork.Products.DeleteMany(store.Products);
            _unitOfWork.Stores.Delete(store);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<List<StoreGet>> GetMany(StoreQueryParams? queryParams)
        {
            var storeEntities = await _unitOfWork.Stores.GetEntities(queryParams);
            return storeEntities.Select(s => TransformOne(s, queryParams)).ToList();
        }

        public async Task<StoreGet> GetOne(long id, StoreQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }


        public async Task<Store> GetEntity(long id, StoreQueryParams? queryParams)
        {
            Store newStore = await _unitOfWork.Stores.GetEntity(id, queryParams);
            if (newStore == null)
            {
                throw new NotFoundException($"store ID '{id}' was not found");
            }
            return newStore;
        }


        public async Task<StoreGet> Save(Store newRow)
        {
            await _unitOfWork.Stores.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }


        public async Task<StoreGet> Update(Store existingRow, StorePatch req)
        { 
            existingRow.Name = req.Name ?? existingRow.Name;
            existingRow.Url = req.Url ?? existingRow.Url;
            existingRow.Description = req.Description ?? existingRow.Description;

            _unitOfWork.Stores.Update(existingRow);
            await _unitOfWork.CompleteAsync();
            return await GetOne(existingRow.Id, null);
        }


        public StoreGet TransformOne(Store row, StoreQueryParams? queryParams)
        {
            queryParams = queryParams == null ? new StoreQueryParams() : queryParams;

            return new StoreGet
            {
                Id = row.Id,
                Name = row.Name,
                Url = row.Url,
                Description = row.Description,
                Platform = queryParams.ShowPlatform == true ? new PlatformGet 
                    { 
                        Id = row.Platform.Id,
                        Name = row.Platform.Name,
                        DisplayName = row.Platform.DisplayName,
                        Description = row.Platform.Description
                    } 
                : null,
                Products = queryParams.ShowProducts == true 
                    ? row.Products.Select(p => new ProductGet 
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Price = p.Price,
                        PublishedAt = p.PublishedAt,
                        Available = p.Available,
                        Description = p.Description,
                    }).ToList()
                    : null,

            };
        }

        public async Task<Store> FindOrShouldCreate(StorePost req)
        {
            StoreQueryParams queryParams = new() { Url = req.Url };
            Store store = await _unitOfWork.Stores.FindOneByParams(queryParams);
            if (store == null)
            {
                store = ConvertToEntity(req);
            }
            return store;
        }
    }
}
