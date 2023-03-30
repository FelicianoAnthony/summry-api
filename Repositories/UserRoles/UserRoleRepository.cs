using LbAutomationPortalApi.Repositories;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.UserRoles
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(SummryContext context) : base(context)
        {
        }
    }
}
