using StarterApi.Repositories.Permissions;
using StarterApi.Repositories.Platforms;
using StarterApi.Repositories.Products;
using StarterApi.Repositories.UserSummryQueries;
using StarterApi.Repositories.RolePermissions;
using StarterApi.Repositories.Roles;
using StarterApi.Repositories.Stores;
using StarterApi.Repositories.UserRoles;
using StarterApi.Repositories.Users;
using StarterApi.Repositories.UserSummryStores;
using StarterApi.Repositories.UserSummries;

namespace StarterApi.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IStoreRepository Stores { get; }

        IPermissionRepository Permissions { get; }

        IPlatformRepository Platforms { get; }

        IProductRepository Products { get; }

        IUserSummryQueryRepository UserSummryQueries{ get; }

        IRolePermissionRepository RolePermissions { get; }

        IRoleRepository Roles { get; }

        IUserRoleRepository UserRoles { get; }

        IUserRepository Users { get; }

        IUserSummryStoreRepository UserSummryStores { get; }

        IUserSummryRepository UserSummries { get; }

        Task CompleteAsync();
    }
}
