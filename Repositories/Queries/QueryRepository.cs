using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.Query;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.Queries
{
    public class QueryRepository : GenericRepository<Query>, IQueryRepository
    {
        private readonly List<string> _userTable = new() { "User" };

        public QueryRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<Query>> GetEntities(QueryQueryParams? queryParams)
        {
            Expression<Func<Query, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<Query> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }

        public async Task<Query> GetEntity(long id, QueryQueryParams? queryParams)
        {
            Expression<Func<Query, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            Query row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }

        // private methods
        private List<string> BuildRelatedEntitiesLookup(QueryQueryParams? queryParams)
        {
            queryParams ??= new QueryQueryParams();
            var relatedTables = new List<string>();
            if (queryParams.ShowUser == true)
            {
                relatedTables.AddRange(_userTable);
            }
            return relatedTables;
        }


        private Expression<Func<Query, bool>> BuildPredicate(QueryQueryParams? queryParams, long? id)
        {
            queryParams = queryParams ?? new QueryQueryParams();

            Expression<Func<Query, bool>> predicate = PredicateBuilder.New<Query>(true);

            if (queryParams.Producer != null)
            {
                predicate = predicate.And(q => q.Producer.Contains(queryParams.Producer));
            }

            if (queryParams.Bottle != null)
            {
                predicate = predicate.And(q => q.Bottle.Contains(queryParams.Bottle));
            }

            if (queryParams.MaxPrice != null)
            {
                predicate = predicate.And(q => q.Price <= queryParams.MaxPrice);
            }

            if (queryParams.MaxRecentMinutes != null)
            {
                predicate = predicate.And(q => q.MostRecentMinutes <= queryParams.MaxRecentMinutes);
            }

            // TODO: this is how authentication at /Queries endpoint is handled...
            if (queryParams.UserId != null)
            { 
                predicate = predicate.And(q => q.UserId == queryParams.UserId); 
            }


            if (id != null)
            {
                predicate = predicate.And(q => q.Id == id);
            }

            return predicate;
        }
    }
}
