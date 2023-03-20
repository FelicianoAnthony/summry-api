using LbAutomationPortalApi.Repositories;
using StarterApi.Repositories.Permissions;
using StarterApi.Repositories.Platforms;
using StarterApi.Repositories.Products;
using StarterApi.Repositories.Queries;
using StarterApi.Repositories.RolePermissions;
using StarterApi.Repositories.Roles;
using StarterApi.Repositories.Stores;
using StarterApi.Repositories.UserRoles;
using StarterApi.Repositories.Users;
using StarterApi.Repositories.UserStores;

namespace StarterApi.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly StarterApiContext _context;

        public IStoreRepository Stores { get; private set; }

        public IPermissionRepository Permissions { get; private set; }

        public IPlatformRepository Platforms { get; private set; }

        public IProductRepository Products { get; private set; }

        public IQueryRepository Queries { get; private set; }

        public IRolePermissionRepository RolePermissions { get; private set; }

        public IRoleRepository Roles { get; private set; }

        public IUserRoleRepository UserRoles { get; private set; }

        public IUserRepository Users { get; private set; }

        public IUserStoreRepository UserStore { get; private set; }

        public UnitOfWork(StarterApiContext context)
        {
            _context = context;
            Stores = new StoreRepository(context);
            Permissions = new PermissionRepository(context);
            Platforms = new PlatformRepository(context);
            Products = new ProductRepository(context);
            RolePermissions = new RolePermissionRepository(context);
            Queries = new QueryRepository(context);
            Roles = new RoleRepository(context);
            UserRoles = new UserRoleRepository(context);
            Users = new UserRepository(context);
            UserStore = new UserStoreRespository(context);
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
