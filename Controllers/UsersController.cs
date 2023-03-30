using Microsoft.AspNetCore.Mvc;
using SummryApi.Entities;
using SummryApi.Services.Users;
using SummryApi.ApiModels.User;
using Microsoft.AspNetCore.Authorization;
using SummryApi.Helpers.AuthHelper;

namespace SummryApi.Controllers
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


        [HttpDelete("me")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            int userId = _authHelpers.GetUserFromJwt(Request.HttpContext.User);

            User user = await _userSvc.GetEntity(userId, new UserQueryParams { ShowSummries = true });
            bool result = await _userSvc.Delete(user);
            return NoContent();
        }

    }
}
