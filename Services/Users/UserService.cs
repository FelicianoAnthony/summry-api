using Newtonsoft.Json;
using StarterApi.ApiModels.Login;
using StarterApi.ApiModels.Query;
using StarterApi.ApiModels.Store;
using StarterApi.ApiModels.User;
using StarterApi.Entities;
using StarterApi.Helpers;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;


namespace StarterApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelper _helperSvc;
        private readonly ILogger<UserService> _logger;
        public UserService(IUnitOfWork unitOfWork, IHelper helperSvc, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _helperSvc = helperSvc;
            _logger = logger;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest request)
        {
            
            _logger.LogInformation($"1) START AUTH ENDPOINT\n{_helperSvc.Serialize(request)}\n\n");
            
            var user = await FindUser(request.Email);
            var passwordMatches = _helperSvc.PasswordMatchesHash(request.Password, user.Password);
            if (!passwordMatches)
            {
                throw new BadHttpRequestException($"Found email but incorrect password.");
            }
            _logger.LogInformation($"2) USER AUTHENTICATED\n{_helperSvc.Serialize(request)}\n\n");

            var token = _helperSvc.GenerateJwtToken(user);
            _logger.LogInformation($"3) JWT GENERATED");
            return new LoginResponse { Token = token };
        }

        public User ConvertToEntity(UserPost req)
        {
            return new User
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                Password = _helperSvc.GenerateHash(req.Password)
            };
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


        public async Task<UserGet> Save(User newRow)
        {
            await _unitOfWork.Users.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }


        // private methods
        private async Task<User> FindUser(string email)
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
                Stores = queryParams.ShowStores == true ? 
                    row.UserStore.Select(us => new StoreGet 
                    {
                        Id = us.Store.Id,
                        Name = us.Store.Name,
                        Url = us.Store.Url,
                        Description = us.Store.Description
                    }).ToList()
                : null,
                Queries = queryParams.ShowQueries == true ?
                    row.Queries.Select(q => new QueryGet
                    {
                        Id = q.Id,
                        Producer = q.Producer,
                        Bottle = q.Bottle,
                        Price = q.Price,
                        MostRecentMinutes = q.MostRecentMinutes,
                    }).ToList()
                : null

            };
        }

    }
}
