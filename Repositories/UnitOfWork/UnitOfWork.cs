using LbAutomationPortalApi.Repositories;
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
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly StarterApiContext _context;

        public IStoreRepository Stores { get; private set; }

        public IPermissionRepository Permissions { get; private set; }

        public IPlatformRepository Platforms { get; private set; }

        public IProductRepository Products { get; private set; }

        public IUserSummryQueryRepository UserSummryQueries { get; private set; }

        public IRolePermissionRepository RolePermissions { get; private set; }

        public IRoleRepository Roles { get; private set; }

        public IUserRoleRepository UserRoles { get; private set; }

        public IUserRepository Users { get; private set; }

        public IUserSummryStoreRepository UserSummryStores { get; private set; }

        public IUserSummryRepository UserSummries { get; private set; }


        public UnitOfWork(StarterApiContext context)
        {
            _context = context;
            Stores = new StoreRepository(context);
            Permissions = new PermissionRepository(context);
            Platforms = new PlatformRepository(context);
            Products = new ProductRepository(context);
            RolePermissions = new RolePermissionRepository(context);
            UserSummryQueries = new UserSummryQueryRepository(context);
            Roles = new RoleRepository(context);
            UserRoles = new UserRoleRepository(context);
            Users = new UserRepository(context);
            UserSummryStores = new UserSummryStoreRepository(context);
            UserSummries = new UserSummryRepository(context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() 
        { 
            _context.Dispose(); 
        }

    }
}
