using StarterApi.ApiModels.Permission;
using StarterApi.ApiModels.Role;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.Permissions
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<List<Permission>> GetEntities(PermissionQueryParams? queryParams);

        Task<Permission> GetEntity(long id, PermissionQueryParams? queryParams);

        Task<Permission> FindOneByProps(PermissionQueryParams queryParams);

    }
}
