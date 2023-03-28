using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.User;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly List<string> _defaultTables = new() 
        {
            "UserSummries.UserSummryQueries",
            "UserSummries.UserSummryStores.Store.Platform"
        };

        public UserRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<User> FindUserByProps(UserQueryParams queryParams)
        {
            Expression<Func<User, bool>> predicate = BuildPredicate(queryParams, null);
            User user = await FindOneWithRelated(predicate, new List<string>());
            return user;
        }


        public async Task<List<User>> GetEntities(UserQueryParams? queryParams)
        {
            Expression<Func<User, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<User> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }


        public async Task<User> GetEntity(long id, UserQueryParams? queryParams)
        {
            Expression<Func<User, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            User row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }


        // private methods
        private List<string> BuildRelatedEntitiesLookup(UserQueryParams? queryParams)
        {
            return _defaultTables;
        }


        private Expression<Func<User, bool>> BuildPredicate(UserQueryParams? queryParams, long? id)
        {
            queryParams = queryParams == null ? new UserQueryParams() : queryParams;

            Expression<Func<User, bool>> predicate = PredicateBuilder.New<User>(true);

            if (queryParams.FirstName != null)
            {
                predicate = predicate.And(u => u.FirstName.Contains(queryParams.FirstName));
            }

            if (queryParams.LastName != null)
            {
                predicate = predicate.And(u => u.LastName.Contains(queryParams.LastName));
            }

            if (queryParams.Email != null)
            {
                predicate = predicate.And(u => u.Email.Contains(queryParams.Email));
            }


            if (id != null)
            {
                predicate = predicate.And(u => u.Id == id);
            }

            return predicate;
        }
    }
}
