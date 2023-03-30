using Microsoft.AspNetCore.Mvc;
using SummryApi.Entities;
using SummryApi.Services.Roles;
using SummryApi.Services.RolePermissions;
using SummryApi.ApiModels.RolePermission;
using SummryApi.Services.Permissions;

namespace SummryApi.Controllers
{
    [Route("api/role-permissions")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionSvc;
        private readonly IRoleService _roleSvc;
        private readonly IPermissionService _permissionSvc;
            

        public RolePermissionsController(IRolePermissionService rolePermissionSvc, IRoleService roleSvc, IPermissionService permissionSvc)
        {
            _rolePermissionSvc = rolePermissionSvc;
            _roleSvc = roleSvc;
            _permissionSvc = permissionSvc;
        }


        [HttpGet]
        public async Task<ActionResult<List<RolePermissionGet>>> GetMany([FromQuery] RolePermissionQueryParams queryParams)
        {
            return await _rolePermissionSvc.GetMany(queryParams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RolePermissionGet>> GetOne(long id, [FromQuery] RolePermissionQueryParams queryParams)
        {
            return await _rolePermissionSvc.GetOne(id, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<RolePermissionGet>> AddOne([FromBody] RolePermissionPost req)
        {
            Role role = await _roleSvc.FindOneByProps(req.Role);
            Permission permission = await _permissionSvc.FindOneByProps(req.Controller, req.Action);

            RolePermission newRow = _rolePermissionSvc.ConvertToEntity(role, permission);
            return await _rolePermissionSvc.Save(newRow);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            RolePermission row = await _rolePermissionSvc.GetEntity(id, null);
            bool result = await _rolePermissionSvc.Delete(row);
            return NoContent();
        }
    }
}
