using StarterApi.ApiModels.Store;
using StarterApi.ApiModels.UserSummry;
using StarterApi.ApiModels.UserSummryQuery;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;
using System.Text.RegularExpressions;

namespace StarterApi.Services.UserSummries
{
    public class UserSummryService : IUserSummryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSummryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserSummry ConvertToEntity(UserSummryPost req, User currUser)
        {
            return new UserSummry
            {
                Title = req.Title,
                Slug = CreateSlug(req.Title),
                UserId = currUser.Id
            };
        }

        public async Task<bool> Delete(UserSummry row)
        {
            _unitOfWork.UserSummryQueries.DeleteMany(row.UserSummryQueries);
            _unitOfWork.UserSummryStores.DeleteMany(row.UserSummryStores);
            _unitOfWork.UserSummries.Delete(row);
            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task<List<UserSummryGet>> GetMany(UserSummryQueryParams? queryParams)
        {
            var entities = await GetEntities(queryParams);
            return entities.Select(s => TransformOne(s, queryParams)).ToList();
        }


        public async Task<List<UserSummry>> GetEntities(UserSummryQueryParams? queryParams)
        { 
            var entities = await _unitOfWork.UserSummries.GetEntities(queryParams);
            return entities.ToList();
        }


        public async Task<UserSummryGet> GetOne(long id, UserSummryQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }


        public async Task<UserSummry> GetEntity(long id, UserSummryQueryParams? queryParams)
        {
            UserSummry userSummry = await _unitOfWork.UserSummries.GetEntity(id, queryParams);
            if (userSummry == null)
            {
                throw new NotFoundException($"User Summry ID '{id}' was not found or you dont have permission to view");
            }
            return userSummry;
        }


        public async Task<UserSummryGet> Save(UserSummry newRow)
        {
            await _unitOfWork.UserSummries.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }


        public async Task<UserSummryGet> Update(UserSummry existingRow, List<UserSummryStore> newStores, List<UserSummryQuery> newQueries)
        {
            _unitOfWork.UserSummryQueries.DeleteMany(existingRow.UserSummryQueries);
            _unitOfWork.UserSummryStores.DeleteMany(existingRow.UserSummryStores);

            existingRow.UserSummryStores = newStores;
            existingRow.UserSummryQueries = newQueries;

            _unitOfWork.UserSummries.Update(existingRow);
            await _unitOfWork.CompleteAsync();

            return await GetOne(existingRow.Id, null);
        }


        public UserSummryGet TransformOne(UserSummry row, UserSummryQueryParams? queryParams)
        {
            queryParams ??= new UserSummryQueryParams();

            return new UserSummryGet
            {
                Id = row.Id,
                Title = row.Title,
                Slug = row.Slug,
                Stores = row.UserSummryStores.Select(p => new StoreGet
                    {
                        Id = p.Store.Id,
                        Url = p.Store.Url
                    }).ToList(),
                Queries = row.UserSummryQueries.Select(p => new UserSummryQueryGet
                    {
                        Id = p.Id,
                        Merchant = p.Merchant,
                        Product = p.Product
                    }).ToList()
                    

            };
        }

        // private 
        private string CreateSlug(string title)
        {
            // https://www.c-sharpcorner.com/blogs/make-url-slugs-in-asp-net-using-c-sharp2
            string output = title.ToLower();

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }

        //public async Task<UserSummry> FindOneByProps(UserSummryQueryParams? queryParams)
        //{
        //    return await _unitOfWork.UserSummryQueries.FindOneByProps(queryParams);
        //}
    }
}
