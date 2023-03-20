using StarterApi.ApiModels.RolePermission;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.RolePermissions
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task<List<RolePermission>> GetEntities(RolePermissionQueryParams? queryParams);

        Task<RolePermission> GetEntity(long id, RolePermissionQueryParams? queryParams);
    }
}
