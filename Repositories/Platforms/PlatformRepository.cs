using LbAutomationPortalApi.Repositories;
using LinqKit;
using SummryApi.ApiModels.Platform;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;
using System.Linq.Expressions;

namespace SummryApi.Repositories.Platforms
{
    public class PlatformRepository : GenericRepository<Platform>, IPlatformRepository
    {
        private readonly List<string> _storesTable = new() { "Stores" };

        public PlatformRepository(SummryContext context) : base(context) { }


        public async Task<Platform> FindByName(string name)
        {
            Expression<Func<Platform, bool>> predicate = BuildPredicate(new PlatformQueryParams { Name = name }, null);
            Platform platform = await FindOneWithRelated(predicate, new List<string>());
            return platform;
        }


        public async Task<List<Platform>> GetEntities(PlatformQueryParams? queryParams)
        {
            Expression<Func<Platform, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<Platform> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }


        public async Task<Platform> GetEntity(long id, PlatformQueryParams? queryParams)
        {
            Expression<Func<Platform, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            Platform row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }


        // private methods
        private List<string> BuildRelatedEntitiesLookup(PlatformQueryParams? queryParams)
        {
            queryParams = queryParams ?? new PlatformQueryParams();
            var relatedTables = new List<string>();
            if (queryParams.ShowStores == true)
            {
                relatedTables.AddRange(_storesTable);
            }
            return relatedTables;
        }


        private Expression<Func<Platform, bool>> BuildPredicate(PlatformQueryParams? queryParams, long? id)
        {
            queryParams = queryParams == null ? new PlatformQueryParams() : queryParams;

            Expression<Func<Platform, bool>> predicate = PredicateBuilder.New<Platform>(true);

            if (queryParams.Name != null)
            {
                predicate = predicate.And(p => p.Name.Contains(queryParams.Name));
            }

            if (queryParams.DisplayName != null)
            {
                predicate = predicate.And(p => p.DisplayName.Contains(queryParams.DisplayName));
            }

            if (queryParams.Description != null)
            {
                predicate = predicate.And(p => p.Description.Contains(queryParams.Description));
            }


            if (id != null)
            {
                predicate = predicate.And(p => p.Id == id);
            }

            return predicate;
        }
    }
}
