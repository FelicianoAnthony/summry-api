using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.UserSummry;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.UserSummries
{
    public class UserSummryRepository : GenericRepository<UserSummry>, IUserSummryRepository
    {
        private readonly List<string> _defaultTables = new() 
        { 
            "User",
            "UserSummryStores.Store.Platform",
            "UserSummryQueries"
        };

        public UserSummryRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<UserSummry>> GetEntities(UserSummryQueryParams? queryParams)
        {
            Expression<Func<UserSummry, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<UserSummry> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }


        public async Task<UserSummry> GetEntity(long id, UserSummryQueryParams? queryParams)
        {
            Expression<Func<UserSummry, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            UserSummry row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }


        // private methods
        private List<string> BuildRelatedEntitiesLookup(UserSummryQueryParams? queryParams)
        {
            return _defaultTables;
        }


        private Expression<Func<UserSummry, bool>> BuildPredicate(UserSummryQueryParams? queryParams, long? id)
        {
            queryParams ??= new UserSummryQueryParams();

            var predicate = PredicateBuilder.New<UserSummry>(true);

            if (queryParams.Title != null)
            {
                predicate = predicate.And(userSum => userSum.Title.Contains(queryParams.Title));
            }

            if (queryParams.Slug != null)
            {
                predicate = predicate.And(userSum => userSum.Slug.Contains(queryParams.Slug));
            }

            if (queryParams.UserId != null)
            {
                predicate = predicate.And(userSum => userSum.User.Id == queryParams.UserId);
            }


            if (id != null)
            {
                predicate = predicate.And(userSum => userSum.Id == id);
            }

            return predicate;
        }
    }
}
