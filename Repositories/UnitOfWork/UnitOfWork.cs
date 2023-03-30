using LbAutomationPortalApi.Repositories;
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
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SummryContext _context;

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


        public UnitOfWork(SummryContext context)
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
