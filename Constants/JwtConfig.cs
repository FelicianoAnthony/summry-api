namespace SummryApi.Constants
{
    public class JwtConfig
    {
        public string? JwtCookieName { get; set; }

        public string? JwtSecret { get; set; }

        public string? JwtMeIdentifier { get; set; }

        public int ExpiresInHours { get; set; }

    }
}
