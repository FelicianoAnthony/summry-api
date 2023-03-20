using StarterApi.ApiModels.Query;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;

namespace StarterApi.Services.Queries
{
    public class QueryService : IQueryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QueryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Query ConvertToEntity(QueryPost postRequest, User user)
        {
            return new Query
            { 
                Producer = postRequest.Producer,
                Bottle = postRequest.Bottle,
                Price = postRequest.Price,
                MostRecentMinutes = postRequest.MostRecentMinutes,
                User = user
            };
        }

        public async Task<bool> Delete(Query entity)
        {
            _unitOfWork.Queries.Delete(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Query> GetEntity(long id, QueryQueryParams? queryParams)
        {
            Query row = await _unitOfWork.Queries.GetEntity(id, queryParams);
            if (row == null)
            {
                throw new NotFoundException($"query ID '{id}' was not found");
            }
            return row;
        }

        public async Task<List<QueryGet>> GetMany(QueryQueryParams? queryParams)
        {
            var entities = await _unitOfWork.Queries.GetEntities(queryParams);
            return entities.Select(s => TransformOne(s, queryParams)).ToList();
        }

        public async Task<QueryGet> GetOne(long id, QueryQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }

        public async Task<QueryGet> Save(Query newRow)
        {
            await _unitOfWork.Queries.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }

        public QueryGet TransformOne(Query row, QueryQueryParams? queryParams)
        {
            queryParams = queryParams == null ? new QueryQueryParams() : queryParams;
            return new QueryGet
            { 
                Id = row.Id,
                Producer = row.Producer,
                Bottle = row.Bottle,
                Price = row.Price,
                MostRecentMinutes = row.MostRecentMinutes,
                User = queryParams.ShowUser == true ?
                    new User 
                    {
                        Id = row.User.Id,
                        FirstName = row.User.FirstName,
                        LastName = row.User.LastName,
                        Email = row.User.Email
                    }
                : null,  
            };
        }


        public async Task<QueryGet> Update(Query existingRow, QueryPatch patchRequest)
        {
            existingRow.Producer = patchRequest.Producer ?? existingRow.Producer;
            existingRow.Bottle = patchRequest.Bottle ?? existingRow.Bottle;
            existingRow.Price = patchRequest.Price ?? existingRow.Price;
            existingRow.MostRecentMinutes = patchRequest.MostRecentMinutes ?? existingRow.MostRecentMinutes;

            _unitOfWork.Queries.Update(existingRow);
            await _unitOfWork.CompleteAsync();
            return await GetOne(existingRow.Id, null);
        }
    }
}
