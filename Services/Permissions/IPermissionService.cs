using StarterApi.ApiModels.Permission;
using StarterApi.Entities;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Permissions
{
    public interface IPermissionService : IBaseService<Permission, PermissionGet, PermissionPost, PermissionPatch, PermissionQueryParams>
    {
        Task<Permission> FindOneByProps(string controller, string action);
    }
}
