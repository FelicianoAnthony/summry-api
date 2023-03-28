//using StarterApi.ApiModels.Store;
//using StarterApi.ApiModels.User;
//using StarterApi.ApiModels.UserStore;
//using StarterApi.Entities;
//using StarterApi.Middlewares.Exceptions;
//using StarterApi.Repositories.UnitOfWork;

//namespace StarterApi.Services.UserSummryStores
//{
//    public class UserSummryStoreService : IUserSummryStoreService
//    {

//        private readonly IUnitOfWork _unitOfWork;

//        public UserSummryStoreService(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }


//        public List<UserSummryStore> ConvertToEntities(List<UserSummryStorePost> requestUserQueries, UserSummry userSummry)
//        {
//            var queries = new List<UserSummryQuery>();
//            foreach (var requestUserQuery in requestUserQueries)
//            {
//                queries.Add(new UserSummryQuery
//                {
//                    Merchant = requestUserQuery.Merchant,
//                    Product = requestUserQuery.Product,
//                    Price = requestUserQuery.Price,
//                    MostRecentMinutes = requestUserQuery.MostRecentMinutes,
//                    UserSummryId = userSummry.Id
//                    // User = user
//                });
//            }

//            return queries;
//        }
//    }
//}
