namespace Inventory.Application.Abstractions.Authentication
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; init; } = string.Empty;

        public string Audience { get; init; } = string.Empty;

        public string Secret { get; init; } = string.Empty;

        public int ExpirationInMinutes { get; init; }

        public string UserName { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;
    }
}