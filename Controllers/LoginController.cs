using Microsoft.AspNetCore.Mvc;
using StarterApi.ApiModels.Login;
using StarterApi.Services.Users;

namespace StarterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userSvc;

        public LoginController(IUserService userSvc)
        {
            _userSvc = userSvc;
        }


        [HttpPost]
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            return await _userSvc.Authenticate(loginRequest);
        }

    }
}
