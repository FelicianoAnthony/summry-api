using StarterApi.Entities;
using System.Security.Claims;

namespace StarterApi.Helpers
{
    public interface IHelper
    {
        string GenerateHash(string password);

        string GenerateJwtToken(User user);

        bool PasswordMatchesHash(string inputPassword, string passwordHash);

        int GetClaimByName(ClaimsPrincipal cookies, string JwtMeIdentifier);

        int TryParseInt(string value);

        int GetUserFromJwt(ClaimsPrincipal userContext);

        string Serialize(dynamic obj);
    }
}
