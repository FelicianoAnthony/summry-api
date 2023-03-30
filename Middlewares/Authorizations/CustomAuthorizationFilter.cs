using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Authentication;

// use it like this above controller method.  no import in program.cs
// [TypeFilter(typeof(CustomAuthorizationFilter))]
namespace SummryApi.Middlewares.Authorizations
{
    // https://referbruv.com/blog/building-custom-responses-for-unauthorized-requests-in-aspnet-core/
    // https://ignas.me/tech/custom-unauthorized-response-body/
    public class CustomAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public AuthorizationPolicy Policy { get; }

        public CustomAuthorizationFilter()
        {
            Policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Allow Anonymous skips all authorization
            //if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            //{
            //    return;
            //}

            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
            var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

            if (authorizeResult.Challenged)
            {
                // this works because error middleware is BEFORE authentication/authorization middleware in program.cs
                throw new AuthenticationException("Token Validation Has Failed. Request Access Denied");
                //// Return custom 401 result
                //context.Result = new JsonResult(new
                //{
                //    Message = "Token Validation Has Failed. Request Access Denied"
                //})
                //{
                //    StatusCode = StatusCodes.Status401Unauthorized
                //};
            }
        }
    }
}
