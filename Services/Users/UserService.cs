using Newtonsoft.Json;
using StarterApi.ApiModels.Login;
using StarterApi.ApiModels.Query;
using StarterApi.ApiModels.Store;
using StarterApi.ApiModels.User;
using StarterApi.Constants;
using StarterApi.Entities;
using StarterApi.Helpers;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;
using System.Text.RegularExpressions;

namespace StarterApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelper _helperSvc;
        private readonly ILogger<UserService> _logger;
        private readonly RegexConfig _regexCfg;

        public UserService(IUnitOfWork unitOfWork, IHelper helperSvc, ILogger<UserService> logger, RegexConfig regexCfg)
        {
            _unitOfWork = unitOfWork;
            _helperSvc = helperSvc;
            _logger = logger;
            _regexCfg = regexCfg;
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

        public async Task<long> CreateUser(UserPost req)
        {
            User newRow = ConvertToEntity(req);
            CheckPasswordStrength(req.Password);
            await DoesUserExist(req.Email);
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
                Password = _helperSvc.GenerateHash(req.Password)
            };
        }

        private async Task DoesUserExist(string email)
        {
            UserQueryParams queryParams = new() { Email = email };
            var user = await _unitOfWork.Users.FindUserByProps(queryParams);
            if (user != null)
            {
                throw new BadHttpRequestException($"{email} already exists");
            }
            return;
        }

        private void CheckPasswordStrength(string password)
        {
            string pattern = $@"{_regexCfg.PasswordStrength}";
            Regex rg = new(pattern);
            Match isMatch = rg.Match(password);
            if (string.IsNullOrEmpty(isMatch.Value))
            {
                string msg = $"At least 1 of the following password requirements were not met\n\n" +
                    $"at least 1 uppercase letter\n" +
                    $"at least 1 special character (!@#$&*)\n" +
                    $"at least 1 digit\n" +
                    $"at least 8 characters in length but no more than 50";
                throw new BadHttpRequestException(msg);
            }
            return;
        }


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

        private async Task<long> Save(User newRow)
        {
            await _unitOfWork.Users.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return newRow.Id;
        }


    }
}
