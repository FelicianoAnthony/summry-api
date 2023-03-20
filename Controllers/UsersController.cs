using Microsoft.AspNetCore.Mvc;
using StarterApi.Entities;
using StarterApi.Services.Users;
using StarterApi.ApiModels.User;
using Microsoft.AspNetCore.Authorization;
using StarterApi.Helpers;
using StarterApi.ApiModels.Store;
using StarterApi.Services.Stores;
using StarterApi.Services.UserStores;
using StarterApi.Services.Platforms;
using StarterApi.ApiModels.UserStore;

namespace StarterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userSvc;
        private readonly IStoreService _storeSvc;
        private readonly IUserStoreService _userStoreSvc;
        private readonly IPlatformService _platformSvc;
        private readonly IHelper _helperSvc;

        public UsersController(
            IUserService userSvc, 
            IStoreService storeSvc,
            IUserStoreService userStoreSvc,
            IPlatformService platformSvc,
            IHelper helperSvc
        )
        {
            _userSvc = userSvc;
            _storeSvc = storeSvc;
            _userStoreSvc = userStoreSvc;
            _platformSvc = platformSvc;
            _helperSvc = helperSvc;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<UserGet>>> GetUsers([FromQuery] UserQueryParams queryParams)
        {
            return await _userSvc.GetMany(queryParams);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserGet>> GetMe([FromQuery] UserQueryParams queryParams)
        {
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            return await _userSvc.GetOne(userId, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<UserGet>> AddUser(UserPost request)
        {
            User newRow = _userSvc.ConvertToEntity(request);
            return await _userSvc.Save(newRow);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            User user = await _userSvc.GetEntity(id, null);
            bool result = await _userSvc.Delete(user);
            return NoContent();
        }


        // stores 
        [Authorize]
        [HttpPost("me/stores")]
        public async Task<ActionResult<UserStoreGet>> AddStoreMe([FromBody] StorePost request)
        {
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);
            Store store = await _storeSvc.FindOrShouldCreate(request);
            store.Platform = await _platformSvc.FindByName(request.Platform);
            // TODO: add logic to validate shopify store vs. citihive store. use DNS records? 

            return await _userStoreSvc.Save(user, store);
        }

        [Authorize]
        [HttpGet("me/stores/{storeId}")]
        public async Task<ActionResult<UserStoreGet>> GetStoreMe(long storeId)
        {
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);

            return await _userStoreSvc.GetOne(user.Id, storeId);
        }


        [Authorize]
        [HttpGet("me/stores")]
        public async Task<ActionResult<List<UserStoreGet>>> GetStoresMe()
        {
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);

            return await _userStoreSvc.GetMany(user.Id);
        }


        [Authorize]
        [HttpDelete("me/stores/{storeId}")]
        public async Task<IActionResult> DeleteStoreMe(long storeId)
        {
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);

            UserStore row = await _userStoreSvc.GetEntity(user.Id, storeId);

            bool result = await _userStoreSvc.Delete(row);
            return NoContent();
        }

    }
}
