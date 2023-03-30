using SummryApi.ApiModels.Role;
using SummryApi.Entities;
using SummryApi.Services.BaseService;

namespace SummryApi.Services.Roles
{
    public interface IRoleService : IBaseService<Role, RoleGet, RolePost, RolePatch, RoleQueryParams>
    {
        Task<Role> FindOneByProps(string name);
    }
}
