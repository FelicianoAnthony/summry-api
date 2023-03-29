using Microsoft.AspNetCore.Mvc;
using StarterApi.Entities;
using StarterApi.Services.Users;
using StarterApi.ApiModels.User;
using Microsoft.AspNetCore.Authorization;
using StarterApi.Helpers.AuthHelper;

namespace StarterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userSvc;
        private readonly IAuthHelpers _authHelpers;

        public UsersController(
            IUserService userSvc,
            IAuthHelpers authHelpers
        )
        {
            _userSvc = userSvc;
            _authHelpers = authHelpers;
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
            int userId = _authHelpers.GetUserFromJwt(Request.HttpContext.User);
            return await _userSvc.GetOne(userId, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<UserGet>> AddUser(UserPost request)
        {
            long newUserId = await _userSvc.CreateUser(request);
            return await _userSvc.GetOne(newUserId, null);
            
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            User user = await _userSvc.GetEntity(id, null);
            bool result = await _userSvc.Delete(user);
            return NoContent();
        }

    }
}
