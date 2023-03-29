using StarterApi.ApiModels.Platform;
using StarterApi.ApiModels.Product;
using StarterApi.ApiModels.Store;
using StarterApi.Constants;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;
using StarterApi.Services.HttpClients;

namespace StarterApi.Services.Stores
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ScrapeApprovalClient _scrapeApprovalClient;
        private readonly ScraperPlatformConfig _platformCfg;

        public StoreService(IUnitOfWork unitOfWork, ScrapeApprovalClient scrapeApprovalClient, ScraperPlatformConfig platformCfg)
        {
            _unitOfWork = unitOfWork;
            _scrapeApprovalClient = scrapeApprovalClient;
            _platformCfg = platformCfg;
        }

        public Store ConvertToEntity(StorePost req, Platform platform)
        {
            return new Store
            { 
                Url = req.Url,
                Platform = platform,
            };
        }

        public async Task<List<UserSummryStore>> ConvertToEntities(List<StorePost> requestUserStores, UserSummry userSummry)
        {
            var userSummryStores = new List<UserSummryStore>();
            foreach (var requestUserStore in requestUserStores)
            {
                // todo: silently fail or not?
                var urlPlatform = await CheckUrlScrapability(requestUserStore.Url);
                if (urlPlatform == null)
                {
                    // throw new Exception($"{requestUserStore.Url} does not match a platform for which scraping logic has been developed");
                    continue;
                }
                Store getStore = await FindOrShouldCreate(requestUserStore, urlPlatform);
                userSummryStores.Add(new UserSummryStore
                {
                    Store = getStore,
                    UserSummryId = userSummry.Id
                });
            }

            return userSummryStores;
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
            existingRow.Url = req.Url ?? existingRow.Url;

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
                Url = row.Url,
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


        // this method is always called once it's known a store can be scraped...
        public async Task<Store> FindOrShouldCreate(StorePost req, Platform platform)
        {
            StoreQueryParams queryParams = new() { Url = req.Url };

            Store store = await _unitOfWork.Stores.FindOneByParams(queryParams);
            if (store == null)
            {
                store = ConvertToEntity(req, platform);
            }
            return store;
        }


        // private 
        public async Task<Platform> CheckUrlScrapability(string url)
        {
            /* todo: talk about preventing adding a store to a summry if scraper cant scrape it
                - commented out code below works.
                - code validates the url being added in a summry can be scraped by python script
             */
            return await _unitOfWork.Platforms.FindByName(_platformCfg.Shopify);
            //var platforms = await _unitOfWork.Platforms.GetEntities(null);

            //Platform urlPlatform = null;
            //foreach (var platform in platforms)
            //{
            //    if (platform.Name.ToLower() == _platformCfg.Shopify)
            //    {
            //        if (await _scrapeApprovalClient.IsShopifyScrapeable(url))
            //        {
            //            urlPlatform = platform;
            //            break;
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception($"DEVELOPER ERROR: logic to scrape '{platform.Name}' not implemented.  delete this platform to fix the error.");
            //    }
            //}
            //return urlPlatform;
        }
    }
}
