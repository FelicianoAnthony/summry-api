using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.UserStores
{
    public class UserStoreRespository : GenericRepository<UserStore>, IUserStoreRepository
    {
        private readonly List<string> _defaultRelatedTables = new() 
        { 
            "User",
            "Store"
        };

        public UserStoreRespository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<UserStore>> GetEntities(long userId)
        {
            Expression<Func<UserStore, bool>> predicate = BuildPredicate(userId, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup();
            IEnumerable<UserStore> userStores = await FindManyWithRelated(predicate, relatedTables);
            return userStores.ToList();
        }

        public async Task<UserStore> GetEntity(long? userId, long? storeId)
        {
            Expression<Func<UserStore, bool>> predicate = BuildPredicate(userId, storeId);
            List<string> relatedTables = BuildRelatedEntitiesLookup();
            UserStore userStore = await FindOneWithRelated(predicate, relatedTables);
            return userStore;
        }


        // private
        private List<string> BuildRelatedEntitiesLookup()
        {
            var relatedTables = _defaultRelatedTables;
            return relatedTables;
        }


        private Expression<Func<UserStore, bool>> BuildPredicate(long? userId, long? storeId)
        {

            Expression<Func<UserStore, bool>> predicate = PredicateBuilder.New<UserStore>(true);

            if (userId != null)
            {
                predicate = predicate.And(us => us.UserId == userId);
            }

            if (storeId != null)
            {
                predicate = predicate.And(us => us.StoreId == storeId);
            }


            return predicate;
        }
    }
}
