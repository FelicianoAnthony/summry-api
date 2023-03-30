//using SummryApi.ApiModels.Store;
//using SummryApi.ApiModels.User;
//using SummryApi.ApiModels.UserStore;
//using SummryApi.Entities;
//using SummryApi.Middlewares.Exceptions;
//using SummryApi.Repositories.UnitOfWork;

//namespace SummryApi.Services.UserSummryStores
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
