using SummryApi.Repositories.Permissions;
using SummryApi.Repositories.Platforms;
using SummryApi.Repositories.Products;
using SummryApi.Repositories.UserSummryQueries;
using SummryApi.Repositories.RolePermissions;
using SummryApi.Repositories.Roles;
using SummryApi.Repositories.Stores;
using SummryApi.Repositories.UserRoles;
using SummryApi.Repositories.Users;
using SummryApi.Repositories.UserSummryStores;
using SummryApi.Repositories.UserSummries;

namespace SummryApi.Repositories.UnitOfWork
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
