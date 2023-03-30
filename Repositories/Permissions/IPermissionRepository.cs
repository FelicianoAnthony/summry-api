using SummryApi.ApiModels.Permission;
using SummryApi.ApiModels.Role;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.Permissions
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<List<Permission>> GetEntities(PermissionQueryParams? queryParams);

        Task<Permission> GetEntity(long id, PermissionQueryParams? queryParams);

        Task<Permission> FindOneByProps(PermissionQueryParams queryParams);

    }
}
