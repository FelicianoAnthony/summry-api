using SummryApi.ApiModels.Role;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.Roles
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<List<Role>> GetEntities(RoleQueryParams? queryParams);

        Task<Role> GetEntity(long id, RoleQueryParams? queryParams);

        Task<Role> FindOneByProps(RoleQueryParams queryParams);

    }
}
