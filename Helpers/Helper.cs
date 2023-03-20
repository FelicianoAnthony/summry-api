using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarterApi.Constants;
using StarterApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StarterApi.Helpers
{
    public class Helper : IHelper
    {
        private readonly JwtConfig _jwtConstants;
        public Helper(JwtConfig jwtConstants)
        {
            _jwtConstants = jwtConstants;
        }

        public string GenerateHash(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }

        public bool PasswordMatchesHash(string inputPassword, string passwordHash)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(inputPassword, passwordHash);
            return verified;
        }

        public int GetClaimByName(ClaimsPrincipal userContext, string JwtMeIdentifier)
        {
            string? userIdString = userContext.FindFirst(JwtMeIdentifier)?.Value;
            if (userIdString == null)
            {
                throw new Exception($"getting secret user from jwt failed! this should never happen unless a cookie name was changed...");
            }

            int.TryParse(userIdString, out int userId);
            // TODO: handle if value not an int. DB query would still fail so nothing would happen <-- prevent this
            return userId;
        }


        public int TryParseInt(string value)
        {
            int.TryParse(value, out int valueInt);
            return valueInt;
        }


        public string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            JwtSecurityTokenHandler tokenHandler = new();
            byte[]? key = Encoding.ASCII.GetBytes(_jwtConstants.JwtSecret);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim(_jwtConstants.JwtMeIdentifier, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int GetUserFromJwt(ClaimsPrincipal userContext)
        {
            int userId = GetClaimByName(userContext, _jwtConstants.JwtMeIdentifier);
            return userId;
        }

        public string Serialize(dynamic obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
