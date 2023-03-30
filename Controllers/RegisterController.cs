using Microsoft.AspNetCore.Mvc;
using SummryApi.Services.Users;
using SummryApi.ApiModels.User;

namespace SummryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userSvc;

        public RegisterController(IUserService userSvc)
        {
            _userSvc = userSvc;
        }
 

        [HttpPost]
        public async Task<ActionResult<UserGet>> AddUser(UserPost request)
        {
            long newUserId = await _userSvc.CreateUser(request);
            return await _userSvc.GetOne(newUserId, null);

        }

    }
}
