using StarterApi.ApiModels.Store;
using StarterApi.ApiModels.User;
using StarterApi.ApiModels.UserStore;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;

namespace StarterApi.Services.UserStores
{
    public class UserStoreService : IUserStoreService
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserStoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<UserStoreGet> Save(User user, Store store)
        {
            UserStore row = new()
            {
                User = user,
                Store = store
            };

            await _unitOfWork.UserStore.Add(row);
            await _unitOfWork.CompleteAsync();

            return await GetOne(row.UserId, row.StoreId);
        }


        public async Task<List<UserStore>> GetEntities(long userId)
        {
            return await _unitOfWork.UserStore.GetEntities(userId);
        }


        public async Task<UserStore> GetEntity(long? userId, long? storeId)
        {
            UserStore userStore = await _unitOfWork.UserStore.GetEntity(userId, storeId);
            if (userStore == null)
            {
                throw new NotFoundException($"user ID '{userId}' && store ID '{storeId}' not found or user doesnt have permission to view store (store exists but not part of current user store collection)");
            }
            return userStore;
        }


        public async Task<List<UserStoreGet>> GetMany(long userId)
        {
            var userStores = await GetEntities(userId);
            return userStores.Select(us => TransformOne(us)).ToList();
        }


        public async Task<UserStoreGet> GetOne(long? userId, long? storeId)
        {
            return TransformOne(await GetEntity(userId, storeId));
        }


        public async Task<bool> Delete(UserStore row)
        {
            _unitOfWork.UserStore.Delete(row);
            await _unitOfWork.CompleteAsync();
            return true;
        }


        // private 
        private UserStoreGet TransformOne(UserStore row)
        {
            return new UserStoreGet
            { 
                User = new UserGet 
                { 
                    Id = row.User.Id,
                    FirstName = row.User.FirstName,
                    LastName = row.User.LastName,
                    Email = row.User.Email,
                },
                Store = new StoreGet
                {
                    Id = row.Store.Id,
                    Name = row.Store.Name,
                    Url = row.Store.Url,
                    Description = row.Store.Description,
                }
            };
        }
    }
}
