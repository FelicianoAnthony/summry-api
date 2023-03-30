using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummryApi.ApiModels.User;
using SummryApi.ApiModels.UserSummry;
using SummryApi.Entities;
using SummryApi.Helpers.AuthHelper;
using SummryApi.Services.Stores;
using SummryApi.Services.Users;
using SummryApi.Services.UserSummries;
using SummryApi.Services.UserSummryQueryService;

namespace SummryApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SummriesController : ControllerBase
    {
        private readonly IUserSummryService _userSummrySvc;
        private readonly IAuthHelpers _authHelpers;
        private readonly IUserService _userSvc;
        private readonly IUserSummryQueryService _userSummryQuerySvc;
        private readonly IStoreService _storeSvc;

        public SummriesController(
            IUserSummryService userSummrySvc,
            IAuthHelpers authHelpers,
            IUserService userSvc,
            IUserSummryQueryService userSummryQuerySvc,
            IStoreService storeSvc
            )
        {
            _userSummrySvc = userSummrySvc;
            _authHelpers = authHelpers;
            _userSvc = userSvc;
            _userSummryQuerySvc = userSummryQuerySvc;
            _storeSvc = storeSvc;
        }


        [HttpGet]
        public async Task<ActionResult<List<UserSummryGet>>> GetMany()
        {
            int userId = _authHelpers.GetUserFromJwt(Request.HttpContext.User);

            UserGet user = await _userSvc.GetOne(userId, null);
            return user.Summries;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserSummryGet>> GetOne(long id)
        {
            int userId = _authHelpers.GetUserFromJwt(Request.HttpContext.User);
            User currUser = await _userSvc.GetEntity(userId, null);

            var queryParams = new UserSummryQueryParams() { UserId = currUser.Id };
            return await _userSummrySvc.GetOne(id, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<UserSummryGet>> AddSummry([FromBody] UserSummryPost req)
        {
            int userId = _authHelpers.GetUserFromJwt(Request.HttpContext.User);
            User currUser = await _userSvc.GetEntity(userId, null);

            UserSummry newRow = _userSummrySvc.ConvertToEntity(req, currUser);
            return await _userSummrySvc.Save(newRow);
        }


        [HttpPut("{id}")]
        public async Task<UserSummryGet> ReplaceSummry(long id, [FromBody] UserSummryPut req)
        {
            int userId = _authHelpers.GetUserFromJwt(Request.HttpContext.User);
            await _userSvc.GetEntity(userId, null);

            var userSummryFilters = new UserSummryQueryParams() { UserId = userId };
            UserSummry userSummry = await _userSummrySvc.GetEntity(id, userSummryFilters);

            List<UserSummryQuery> newQueries = _userSummryQuerySvc.ConvertToEntities(req.Queries, userSummry);
            List<UserSummryStore> newStores = await _storeSvc.ConvertToEntities(req.Stores, userSummry);

            return await _userSummrySvc.Update(userSummry, newStores, newQueries);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            int userId = _authHelpers.GetUserFromJwt(Request.HttpContext.User);
            await _userSvc.GetEntity(userId, null);

            var userSummryFilters = new UserSummryQueryParams() { UserId = userId };
            UserSummry userSummry = await _userSummrySvc.GetEntity(id, userSummryFilters);

            await _userSummrySvc.Delete(userSummry);
            return NoContent();
        }

    }
}
