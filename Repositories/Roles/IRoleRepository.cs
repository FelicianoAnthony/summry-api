using StarterApi.ApiModels.Role;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.Roles
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<List<Role>> GetEntities(RoleQueryParams? queryParams);

        Task<Role> GetEntity(long id, RoleQueryParams? queryParams);

        Task<Role> FindOneByProps(RoleQueryParams queryParams);

    }
}
