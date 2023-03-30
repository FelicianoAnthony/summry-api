using System.Security.Authentication;
// https://stackoverflow.com/questions/62199488/how-to-enable-custom-authorization-messages
namespace SummryApi.Middlewares.Authorizations
{
    public class AuthenticationErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                throw new AuthenticationException("Token Validation Has Failed. Request Access Denied");
            }
        }
    }
}
