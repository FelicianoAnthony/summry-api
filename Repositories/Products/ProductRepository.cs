using LbAutomationPortalApi.Repositories;
using LinqKit;
using SummryApi.ApiModels.Product;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;
using System.Linq.Expressions;

namespace SummryApi.Repositories.Products
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {

        private readonly List<string> _storeTable = new() { "Store" };

        public ProductRepository(SummryContext context) : base(context) { }



        public async Task<List<Product>> GetEntities(ProductQueryParams? queryParams)
        {
            Expression<Func<Product, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<Product> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }


        public async Task<Product> GetEntity(long id, ProductQueryParams? queryParams)
        {
            Expression<Func<Product, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            Product row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }

        private List<string> BuildRelatedEntitiesLookup(ProductQueryParams? queryParams)
        {
            queryParams = queryParams ?? new ProductQueryParams();
            var relatedTables = new List<string>();
            if (queryParams.ShowStore == true)
            {
                relatedTables.AddRange(_storeTable);
            }
            return relatedTables;
        }


        private Expression<Func<Product, bool>> BuildPredicate(ProductQueryParams? queryParams, long? id)
        {
            queryParams = queryParams ?? new ProductQueryParams();

            Expression<Func<Product, bool>> predicate = PredicateBuilder.New<Product>(true);

            if (queryParams.Title != null)
            {
                predicate = predicate.And(p => p.Title.Contains(queryParams.Title));
            }

            if (queryParams.Price != null)
            {
                predicate = predicate.And(p => p.Price == queryParams.Price);
            }

            if (queryParams.PublishedAtLaterThan != null)
            {
                predicate = predicate.And(p => p.PublishedAt >= queryParams.PublishedAtLaterThan);
            }

            if (queryParams.Available != null)
            {
                predicate = predicate.And(p => p.Available == queryParams.Available);
            }

            if (queryParams.Description != null)
            {
                predicate = predicate.And(p => p.Description.Contains(queryParams.Description));
            }

            if (queryParams.StoreId != null)
            {
                predicate = predicate.And(p => p.StoreId == queryParams.StoreId);
            }


            if (id != null)
            {
                predicate = predicate.And(s => s.Id == id);
            }

            return predicate;
        }
    }

}
