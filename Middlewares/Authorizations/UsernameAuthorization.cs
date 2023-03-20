using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StarterApi.Middlewares.Authorizations
{
    public class UsernameAuthorization : AuthorizationHandler<UserNameRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserNameRequirement requirement)
        {

            string? jwtUser = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (jwtUser == requirement.UserName)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class UserNameRequirement : IAuthorizationRequirement
    {
        public UserNameRequirement(string username)
        {
            UserName = username;
        }

        public string UserName { get; set; }
    }
}
