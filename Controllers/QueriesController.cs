using Microsoft.AspNetCore.Mvc;
using StarterApi.Entities;
using StarterApi.Services.Queries;
using StarterApi.ApiModels.Query;
using StarterApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using StarterApi.Helpers;

namespace StarterApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly IQueryService _querySvc;
        private readonly IUserService _userSvc;
        private readonly IHelper _helperSvc;

        public QueriesController(IQueryService querySvc, IUserService userSvc, IHelper helperSvc)
        {
            _querySvc = querySvc;
            _userSvc = userSvc;
            _helperSvc = helperSvc;
        }


        [HttpGet]
        public async Task<ActionResult<List<QueryGet>>> GetMany([FromQuery] QueryQueryParams queryParams)
        {
            // TODO: add this to middleware & add 'userId' as property on request.httpContext object...
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);
            queryParams.UserId = user.Id;

            return await _querySvc.GetMany(queryParams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<QueryGet>> GetOne(long id, [FromQuery] QueryQueryParams queryParams)
        {
            // TODO: add this to middleware & add 'userId' as property on request.httpContext object...
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);
            queryParams.UserId = user.Id;

            return await _querySvc.GetOne(id, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<QueryGet>> AddOne([FromBody] QueryPost platform)
        {
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);

            Query newRow = _querySvc.ConvertToEntity(platform, user);
            return await _querySvc.Save(newRow);
        }


        [HttpPatch("{id}")]
        public async Task<QueryGet> ModifyOne(long id, [FromBody] QueryPatch req)
        {
            // TODO: add this to middleware & add 'userId' as property on request.httpContext object...
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);
            var queryParams = new QueryQueryParams { UserId = user.Id };

            Query existingRow = await _querySvc.GetEntity(id, queryParams);
            return await _querySvc.Update(existingRow, req);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(long id)
        {
            // TODO: add this to middleware & add 'userId' as property on request.httpContext object...
            int userId = _helperSvc.GetUserFromJwt(Request.HttpContext.User);
            User user = await _userSvc.GetEntity(userId, null);
            var queryParams = new QueryQueryParams { UserId = user.Id };

            Query entity = await _querySvc.GetEntity(id, queryParams);
            bool result = await _querySvc.Delete(entity);
            return NoContent();
        }
    }
}
