using SummryApi.ApiModels.Permission;
using SummryApi.Entities;
using SummryApi.Services.BaseService;

namespace SummryApi.Services.Permissions
{
    public interface IPermissionService : IBaseService<Permission, PermissionGet, PermissionPost, PermissionPatch, PermissionQueryParams>
    {
        Task<Permission> FindOneByProps(string controller, string action);
    }
}
