using StarterApi.ApiModels.UserSummryQuery;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;

namespace StarterApi.Services.UserSummryQueryService
{
    public class UserSummryQueryService : IUserSummryQueryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSummryQueryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<UserSummryQuery> ConvertToEntities(List<UserSummryQueryPost> requestUserQueries, UserSummry userSummry)
        {
            var queries = new List<UserSummryQuery>();
            foreach(var requestUserQuery in requestUserQueries)
            {
                queries.Add(new UserSummryQuery
                {
                    Merchant = requestUserQuery.Merchant,
                    Product = requestUserQuery.Product,
                    Price = requestUserQuery.Price,
                    MostRecentMinutes = requestUserQuery.MostRecentMinutes,
                    UserSummryId = userSummry.Id
                });
            }

            return queries; 
        }

        public async Task<bool> Delete(UserSummryQuery entity)
        {
            _unitOfWork.UserSummryQueries.Delete(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<UserSummryQuery> GetEntity(long id, UserSummryQueryFilters? queryParams)
        {
            UserSummryQuery row = await _unitOfWork.UserSummryQueries.GetEntity(id, queryParams);
            if (row == null)
            {
                throw new NotFoundException($"user summry query ID '{id}' was not found");
            }
            return row;
        }

        public async Task<List<UserSummryQueryGet>> GetMany(UserSummryQueryFilters? queryParams)
        {
            var entities = await _unitOfWork.UserSummryQueries.GetEntities(queryParams);
            return entities.Select(s => TransformOne(s, queryParams)).ToList();
        }

        public async Task<UserSummryQueryGet> GetOne(long id, UserSummryQueryFilters? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }

        public async Task<UserSummryQueryGet> Save(UserSummryQuery newRow)
        {
            await _unitOfWork.UserSummryQueries.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }

        public UserSummryQueryGet TransformOne(UserSummryQuery row, UserSummryQueryFilters? queryParams)
        {
            queryParams = queryParams == null ? new UserSummryQueryFilters() : queryParams;
            return new UserSummryQueryGet
            { 
                Id = row.Id,
                Merchant = row.Merchant,
                Product = row.Product,
                Price = row.Price,
                MostRecentMinutes = row.MostRecentMinutes,
                User = queryParams.ShowUser == true ?
                    new User 
                    {
                        Id = row.UserSummry.User.Id,
                        FirstName = row.UserSummry.User.FirstName,
                        LastName = row.UserSummry.User.LastName,
                        Email = row.UserSummry.User.Email
                    }
                : null,  
            };
        }


        public async Task<UserSummryQueryGet> Update(UserSummryQuery existingRow, UserSummryQueryPatch patchRequest)
        {
            existingRow.Merchant = patchRequest.Merchant ?? existingRow.Merchant;
            existingRow.Product = patchRequest.Product ?? existingRow.Product;
            existingRow.Price = patchRequest.Price ?? existingRow.Price;
            existingRow.MostRecentMinutes = patchRequest.MostRecentMinutes ?? existingRow.MostRecentMinutes;

            _unitOfWork.UserSummryQueries.Update(existingRow);
            await _unitOfWork.CompleteAsync();
            return await GetOne(existingRow.Id, null);
        }
    }
}
