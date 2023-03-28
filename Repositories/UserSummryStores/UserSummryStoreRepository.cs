using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.UserSummryStores
{
    public class UserSummryStoreRepository : GenericRepository<UserSummryStore>, IUserSummryStoreRepository
    {
        private readonly List<string> _defaultRelatedTables = new() 
        {
            "UserSummry.User",
            "Store"
        };

        public UserSummryStoreRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<UserSummryStore>> GetEntities(long userId)
        {
            Expression<Func<UserSummryStore, bool>> predicate = BuildPredicate(userId, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup();
            IEnumerable<UserSummryStore> userStores = await FindManyWithRelated(predicate, relatedTables);
            return userStores.ToList();
        }

        public async Task<UserSummryStore> GetEntity(long? userId, long? storeId)
        {
            Expression<Func<UserSummryStore, bool>> predicate = BuildPredicate(userId, storeId);
            List<string> relatedTables = BuildRelatedEntitiesLookup();
            UserSummryStore userStore = await FindOneWithRelated(predicate, relatedTables);
            return userStore;
        }


        // private
        private List<string> BuildRelatedEntitiesLookup()
        {
            return _defaultRelatedTables;
        }


        private Expression<Func<UserSummryStore, bool>> BuildPredicate(long? userId, long? storeId)
        {

            Expression<Func<UserSummryStore, bool>> predicate = PredicateBuilder.New<UserSummryStore>(true);

            if (userId != null)
            {
                predicate = predicate.And(us => us.UserSummry.User.Id == userId);
            }

            if (storeId != null)
            {
                predicate = predicate.And(us => us.StoreId == storeId);
            }


            return predicate;
        }
    }
}
