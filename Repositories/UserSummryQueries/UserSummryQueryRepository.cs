using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.UserSummryQuery;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.UserSummryQueries
{
    public class UserSummryQueryRepository : GenericRepository<UserSummryQuery>, IUserSummryQueryRepository
    {
        private readonly List<string> _userTable = new() { "UserSummry.User" };

        public UserSummryQueryRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<UserSummryQuery>> GetEntities(UserSummryQueryFilters? queryParams)
        {
            Expression<Func<UserSummryQuery, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<UserSummryQuery> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }

        public async Task<UserSummryQuery> GetEntity(long id, UserSummryQueryFilters? queryParams)
        {
            Expression<Func<UserSummryQuery, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            UserSummryQuery row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }

        // private methods
        private List<string> BuildRelatedEntitiesLookup(UserSummryQueryFilters? queryParams)
        {
            queryParams ??= new UserSummryQueryFilters();
            var relatedTables = new List<string>();
            if (queryParams.ShowUser == true)
            {
                relatedTables.AddRange(_userTable);
            }
            return relatedTables;
        }


        private Expression<Func<UserSummryQuery, bool>> BuildPredicate(UserSummryQueryFilters? queryParams, long? id)
        {
            queryParams = queryParams ?? new UserSummryQueryFilters();

            Expression<Func<UserSummryQuery, bool>> predicate = PredicateBuilder.New<UserSummryQuery>(true);

            if (queryParams.Merchant != null)
            {
                predicate = predicate.And(q => q.Merchant.Contains(queryParams.Merchant));
            }

            if (queryParams.Product != null)
            {
                predicate = predicate.And(q => q.Product.Contains(queryParams.Product));
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
                predicate = predicate.And(q => q.UserSummry.User.Id == queryParams.UserId); 
            }


            if (id != null)
            {
                predicate = predicate.And(q => q.Id == id);
            }

            return predicate;
        }
    }
}
