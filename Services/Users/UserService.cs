using SummryApi.ApiModels.Login;
using SummryApi.ApiModels.Store;
using SummryApi.ApiModels.User;
using SummryApi.ApiModels.UserSummry;
using SummryApi.ApiModels.UserSummryQuery;
using SummryApi.Entities;
using SummryApi.Helpers.AuthHelper;
using SummryApi.Middlewares.Exceptions;
using SummryApi.Repositories.UnitOfWork;

namespace SummryApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthHelpers _authHelper;

        public UserService(IUnitOfWork unitOfWork, IAuthHelpers authHelper)
        {
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest request)
        {            
            var user = await UserMustExist(request.Email);
            var passwordMatches = _authHelper.PasswordMatchesHash(request.Password, user.Password);
            if (!passwordMatches)
            {
                throw new BadHttpRequestException($"Found email but incorrect password.");
            }

            var token = _authHelper.GenerateJwtToken(user);
            return new LoginResponse { Token = token };
        }

        public async Task<long> CreateUser(UserPost req)
        {
            _authHelper.CheckPasswordStrength(req.Password);
            await UserMustNotExist(req.Email);
            User newRow = ConvertToEntity(req);
            return await Save(newRow);
        }


        public async Task<bool> Delete(User store)
        {
            _unitOfWork.Users.Delete(store);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<List<UserGet>> GetMany(UserQueryParams? queryParams)
        {
            var userEntities = await _unitOfWork.Users.GetEntities(queryParams);
            return userEntities.Select(s => TransformOne(s, queryParams)).ToList();
        }

        public async Task<UserGet> GetOne(long id, UserQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }


        public async Task<User> GetEntity(long id, UserQueryParams? queryParams)
        {
            User newUser = await _unitOfWork.Users.GetEntity(id, queryParams);
            if (newUser == null)
            {
                throw new NotFoundException($"user ID '{id}' was not found");
            }
            return newUser;
        }



        // private methods
        private User ConvertToEntity(UserPost req)
        {
            return new User
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                Password = _authHelper.GenerateHash(req.Password)
            };
        }

        private async Task UserMustNotExist(string email)
        {
            UserQueryParams queryParams = new() { Email = email };
            var user = await _unitOfWork.Users.FindUserByProps(queryParams);
            if (user != null)
            {
                throw new BadHttpRequestException($"{email} already exists");
            }
            return;
        }


        private async Task<User> UserMustExist(string email)
        {
            UserQueryParams queryParams = new(){ Email = email };
            var user = await _unitOfWork.Users.FindUserByProps(queryParams);
            if (user == null)
            {
                throw new BadHttpRequestException($"Email '{email}' does not exist. Please sign up first.");
            }
            return user;
        }


        private UserGet TransformOne(User row, UserQueryParams queryParams)
        {
            queryParams = queryParams == null ? new UserQueryParams() : queryParams;

            return new UserGet
            {
                Id = row.Id,
                FirstName = row.FirstName,
                LastName = row.LastName,
                Email = row.Email,
                Summries = row.UserSummries.Select(userSummry => new UserSummryGet {
                    Id = userSummry.Id,
                    Title = userSummry.Title,
                    Slug = userSummry.Slug,

                    Queries = userSummry.UserSummryQueries.Select(userQuery => new UserSummryQueryGet
                    { 
                        Id = userQuery.Id,
                        Merchant = userQuery.Merchant,
                        Product = userQuery.Product,
                        Price = userQuery.Price,
                        MostRecentMinutes = userQuery.MostRecentMinutes
                    }).ToList(),

                    Stores = userSummry.UserSummryStores.Select(userStore => new StoreGet 
                    { 
                        Id = userStore.Store.Id,
                        Url = userStore.Store.Url
                    }).ToList()

                }).ToList()
            };
        }

        private async Task<long> Save(User newRow)
        {
            await _unitOfWork.Users.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return newRow.Id;
        }


    }
}
