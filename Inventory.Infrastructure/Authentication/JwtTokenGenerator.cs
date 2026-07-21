using Inventory.Application.Abstractions.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventory.Infrastructure.Authentication
{
    public sealed class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IJwtConfiguration _jwtConfiguration;

        public JwtTokenGenerator(
            IJwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }

        public string GenerateToken(
            string userName,
            IEnumerable<Claim> claims)
        {
            var options = _jwtConfiguration.Current;

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.Secret));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var jwtClaims = new List<Claim>(claims)
            {
                new(JwtRegisteredClaimNames.Sub, userName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: jwtClaims,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}