using Microsoft.IdentityModel.Tokens;
using SummryApi.Constants;
using SummryApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace SummryApi.Helpers.AuthHelper
{
    public class AuthHelpers : IAuthHelpers
    {
        private readonly JwtConfig _jwtCfg;
        private readonly RegexConfig _regexCfg;

        public AuthHelpers(JwtConfig jwtCfg, RegexConfig regexCfg)
        {
            _jwtCfg = jwtCfg;
            _regexCfg = regexCfg;
        }


        public string GenerateHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        public bool PasswordMatchesHash(string inputPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, passwordHash);
        }


        public string GenerateJwtToken(User user)
        {
            // generate token that is valid for 24 hours with userId encoded in token
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_jwtCfg.JwtSecret);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim(_jwtCfg.JwtMeIdentifier, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(_jwtCfg.ExpiresInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public int GetUserFromJwt(ClaimsPrincipal userContext)
        {
            int userId = GetClaimByName(userContext, _jwtCfg.JwtMeIdentifier);
            return userId;
        }


        public void CheckPasswordStrength(string password)
        {
            string pattern = $@"{_regexCfg.PasswordStrength}";
            Regex rg = new(pattern);
            Match isMatch = rg.Match(password);
            if (string.IsNullOrEmpty(isMatch.Value))
            {
                string msg = $"At least 1 of the following password requirements were not met\n\n" +
                    $"at least 1 uppercase letter\n" +
                    $"at least 1 special character (!@#$&*)\n" +
                    $"at least 1 digit\n" +
                    $"at least 8 characters in length but no more than 50";
                throw new BadHttpRequestException(msg);
            }
            return;
        }


        // private 
        private int GetClaimByName(ClaimsPrincipal userContext, string jwtMeIdentifier)
        {
            string userIdString = userContext.FindFirst(jwtMeIdentifier)?.Value;
            if (userIdString == null)
            {
                throw new Exception($"getting secret user from jwt failed! this should never happen unless a cookie name was changed...");
            }

            int.TryParse(userIdString, out int userId);
            // TODO: handle if value not an int. DB query would still fail so nothing would happen <-- prevent this
            return userId;
        }

    }
}
