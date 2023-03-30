using SummryApi.Entities;
using System.Security.Claims;

namespace SummryApi.Helpers.AuthHelper
{
    public interface IAuthHelpers
    {
        string GenerateHash(string password);

        string GenerateJwtToken(User user);

        bool PasswordMatchesHash(string inputPassword, string passwordHash);

        public int GetUserFromJwt(ClaimsPrincipal userContext);

        void CheckPasswordStrength(string password);

    }
}
