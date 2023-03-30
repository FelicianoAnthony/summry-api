using SummryApi.ApiModels.RolePermission;
using SummryApi.Entities;

namespace SummryApi.Services.RolePermissions
{
    public interface IRolePermissionService
    {
        RolePermission ConvertToEntity(Role role, Permission permission);

        Task<bool> Delete(RolePermission entity);

        Task<List<RolePermissionGet>> GetMany(RolePermissionQueryParams? queryParams);

        Task<RolePermissionGet> GetOne(long id, RolePermissionQueryParams? queryParams);

        Task<RolePermission> GetEntity(long id, RolePermissionQueryParams? queryParams);

        Task<RolePermissionGet> Save(RolePermission newRow);

    }
}
