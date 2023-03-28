using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.Store;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.Stores
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {

        private readonly List<string> _productsTable = new() { "Products" };

        private readonly List<string> _defaultRelatedTables = new() { "Platform" };

        public StoreRepository(StarterApiContext context) : base(context) { }



        public async Task<List<Store>> GetEntities(StoreQueryParams? queryParams)
        {
            Expression<Func<Store, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<Store> stores = await FindManyWithRelated(predicate, relatedTables);
            return stores.ToList();
        }


        public async Task<Store> GetEntity(long id, StoreQueryParams? queryParams)
        {
            Expression<Func<Store, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            Store store = await FindOneWithRelated(predicate, relatedTables);
            return store;
        }

        private List<string> BuildRelatedEntitiesLookup(StoreQueryParams? queryParams)
        {
            queryParams = queryParams ?? new StoreQueryParams();
            var relatedTables = _defaultRelatedTables;
            if (queryParams.ShowProducts == true)
            {
                relatedTables.AddRange(_productsTable);
            }

            return relatedTables;
        }

        public async Task<Store> FindOneByParams(StoreQueryParams? queryParams)
        {
            Expression<Func<Store, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            return await FindOneWithRelated(predicate, relatedTables);
        }


        private Expression<Func<Store, bool>> BuildPredicate(StoreQueryParams? queryParams, long? id)
        {
            queryParams = queryParams == null ? new StoreQueryParams() : queryParams;

            Expression<Func<Store, bool>> predicate = PredicateBuilder.New<Store>(true);

            if (queryParams.Url != null)
            {
                predicate = predicate.And(s => s.Url.Contains(queryParams.Url));
            }

            if (id != null)
            {
                predicate = predicate.And(s => s.Id == id);
            }


            // query foreign keys
            if (queryParams.PlatformId != null)
            {
                predicate = predicate.And(s => s.PlatformId == queryParams.PlatformId);
            }

            if (queryParams.Platform != null)
            {
                predicate = predicate.And(s => s.Platform.Name == queryParams.Platform);
            }

            return predicate;
        }
    }
}
