using System.Security.Claims;

namespace Inventory.Application.Abstractions.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(
            string userName,
            IEnumerable<Claim> claims);
    }
}