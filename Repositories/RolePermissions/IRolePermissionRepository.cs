using SummryApi.ApiModels.RolePermission;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.RolePermissions
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task<List<RolePermission>> GetEntities(RolePermissionQueryParams? queryParams);

        Task<RolePermission> GetEntity(long id, RolePermissionQueryParams? queryParams);
    }
}
