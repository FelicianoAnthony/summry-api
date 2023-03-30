using Microsoft.AspNetCore.Mvc;
using SummryApi.Entities;
using SummryApi.ApiModels.Permission;
using SummryApi.Services.Permissions;

namespace SummryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionSvc;

        public PermissionsController(IPermissionService permissionSvc)
        {
            _permissionSvc = permissionSvc;
        }


        [HttpGet]
        public async Task<ActionResult<List<PermissionGet>>> GetMany([FromQuery] PermissionQueryParams queryParams)
        {
            return await _permissionSvc.GetMany(queryParams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionGet>> GetOne(long id, [FromQuery] PermissionQueryParams queryParams)
        {
            return await _permissionSvc.GetOne(id, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<PermissionGet>> AddOne([FromBody] PermissionPost req)
        {
            Permission permission = _permissionSvc.ConvertToEntity(req);
            return await _permissionSvc.Save(permission);
        }


        [HttpPatch("{id}")]
        public async Task<PermissionGet> ModifyOne(long id, [FromBody] PermissionPatch req)
        {
            Permission existingRow = await _permissionSvc.GetEntity(id, null);
            return await _permissionSvc.Update(existingRow, req);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            Permission row = await _permissionSvc.GetEntity(id, null);
            bool result = await _permissionSvc.Delete(row);
            return NoContent();
        }
    }
}
