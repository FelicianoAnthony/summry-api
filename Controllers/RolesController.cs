using Microsoft.AspNetCore.Mvc;
using StarterApi.Entities;
using StarterApi.ApiModels.Role;
using StarterApi.Services.Roles;

namespace StarterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleSvc;

        public RolesController(IRoleService roleSvc)
        {
            _roleSvc = roleSvc;
        }


        [HttpGet]
        public async Task<ActionResult<List<RoleGet>>> GetMany([FromQuery] RoleQueryParams queryParams)
        {
            return await _roleSvc.GetMany(queryParams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RoleGet>> GetOne(long id, [FromQuery] RoleQueryParams queryParams)
        {
            return await _roleSvc.GetOne(id, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<RoleGet>> AddOne([FromBody] RolePost req)
        {
            Role newRow = _roleSvc.ConvertToEntity(req);
            return await _roleSvc.Save(newRow);
        }


        [HttpPatch("{id}")]
        public async Task<RoleGet> ModifyOne(long id, [FromBody] RolePatch req)
        {
            Role existingRow = await _roleSvc.GetEntity(id, null);
            return await _roleSvc.Update(existingRow, req);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            Role row = await _roleSvc.GetEntity(id, null);
            bool result = await _roleSvc.Delete(row);
            return NoContent();
        }
    }
}
