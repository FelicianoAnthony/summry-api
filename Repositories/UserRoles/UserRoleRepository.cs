using LbAutomationPortalApi.Repositories;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.UserRoles
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(StarterApiContext context) : base(context)
        {
        }
    }
}
