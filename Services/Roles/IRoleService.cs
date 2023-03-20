using StarterApi.ApiModels.Role;
using StarterApi.Entities;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Roles
{
    public interface IRoleService : IBaseService<Role, RoleGet, RolePost, RolePatch, RoleQueryParams>
    {
        Task<Role> FindOneByProps(string name);
    }
}
