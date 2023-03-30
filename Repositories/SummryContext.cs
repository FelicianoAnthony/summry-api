using Microsoft.EntityFrameworkCore;
using SummryApi.Entities;
using SummryApi.Entities.Base;

namespace LbAutomationPortalApi.Repositories
{
    public partial class SummryContext : DbContext
    {
        public IConfiguration Configuration { get; }


        public SummryContext()
        {
        }

        public SummryContext(DbContextOptions<SummryContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<Store> Stores{ get; set; }

        public virtual DbSet<Product> Products { get; set; }


        public DbSet<Platform> Platform { get; set; }

        public DbSet<UserSummryQuery> UserSummryQueries { get; set; }

        public DbSet<UserSummryStore> UserSummryStores { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserSummry> UserSummries { get; set; }

        public DbSet<User> Users { get; set; } // change this to Users to match DB...

        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Configuration.GetValue<string>("DefaultSchema"));

            // non-nullable columns
            modelBuilder.Entity<Store>().Property(a => a.Url).IsRequired();
            modelBuilder.Entity<Platform>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Permission>().Property(p => p.Controller).IsRequired();
            modelBuilder.Entity<Permission>().Property(p => p.Action).IsRequired();


            // uniqueness constraints
            modelBuilder.Entity<Store>().HasIndex(a => a.Url).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Platform>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();

            // composite unique index 
            modelBuilder.Entity<UserSummryStore>().HasIndex(uss => new { uss.StoreId, uss.UserSummryId }).IsUnique();
            modelBuilder.Entity<Permission>().HasIndex(p => new { p.Controller, p.Action }).IsUnique();
            modelBuilder.Entity<RolePermission>().HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
            modelBuilder.Entity<UserRole>().HasIndex(urp => new { urp.UserId, urp.RoleId }).IsUnique();
            modelBuilder.Entity<UserSummry>().HasIndex(us => new { us.UserId, us.Title }).IsUnique();

            // default boolean values 
            modelBuilder.Entity<UserSummry>().Property(us => us.IsPaused).HasDefaultValue(false);
            modelBuilder.Entity<UserSummryStore>().Property(uss => uss.IsPaused).HasDefaultValue(false);
            modelBuilder.Entity<UserSummryQuery>().Property(usq => usq.IsPaused).HasDefaultValue(false);



            // don't delete a store if there's at least 1 product associated to it 
            modelBuilder.Entity<Store>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Store)
                .OnDelete(DeleteBehavior.Restrict);

            // don't delete a store if there's at least 1 UserSummryStore associated to it
            modelBuilder.Entity<Store>()
                .HasMany(s => s.UserSummryStore)
                .WithOne(uss => uss.Store)
                .OnDelete(DeleteBehavior.Restrict);

            // don't delete a user if there's at least 1 UserSummry associated to it
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserSummries)
                .WithOne(userSum => userSum.User)
                .OnDelete(DeleteBehavior.Restrict);

            // don't delete a user if there's at least 1 role permissionId associated to it
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(urp => urp.User)
                .OnDelete(DeleteBehavior.Restrict);

            // dont delete a UserSummry if there's at least 1 query associated with it
            modelBuilder.Entity<UserSummry>()
                .HasMany(us => us.UserSummryQueries)
                .WithOne(usq => usq.UserSummry)
                .OnDelete(DeleteBehavior.Restrict);

            // dont delete a UserSummry if there's at least 1 store associated with it
            modelBuilder.Entity<UserSummry>()
                .HasMany(us => us.UserSummryStores)
                .WithOne(uss => uss.UserSummry)
                .OnDelete(DeleteBehavior.Restrict);

            // don't delete a platform if there's at least 1 store associated to it
            modelBuilder.Entity<Platform>()
                .HasMany(p => p.Stores)
                .WithOne(s => s.Platform)
                .OnDelete(DeleteBehavior.Restrict);

            // don't delete a role if there's at least 1 rolePermissionId associated to it
            modelBuilder.Entity<Role>()
                .HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .OnDelete(DeleteBehavior.Restrict);

            // don't delete a permission if there's at least 1 rolePermissionId associated to it
            modelBuilder.Entity<Permission>()
                .HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Permission)
                .OnDelete(DeleteBehavior.Restrict);

            // TODO: may not need this...
            //// don't delete a RolePermission if there's at least 1 UserRolePermissionId associated to it
            //modelBuilder.Entity<RolePermission>()
            //    .HasMany(rp => rp.UserRolePermission)
            //    .WithMany(urp => urp.UserRolePermission)
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                // for entities that inherit from BaseEntity,
                // set UpdatedOn / CreatedOn appropriately
                if (entry.Entity is BaseTimestamp trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.UpdatedOn = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            entry.Property("CreatedOn").IsModified = false;
                            entry.Property("DeletedOn").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            trackable.CreatedOn = utcNow;
                            trackable.UpdatedOn = utcNow;

                            entry.Property("DeletedOn").IsModified = false;
                            break;

                    }
                }
            }
        }

    }
}

