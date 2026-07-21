using Inventory.Application.Abstractions.Authentication;
using Microsoft.Extensions.Options;

namespace Inventory.Infrastructure.Authentication
{
    public sealed class JwtConfiguration : IJwtConfiguration
    {
        public JwtOptions Current { get; }

        public JwtConfiguration(
            IOptions<JwtOptions> options)
        {
            Current = options.Value;
        }
    }
}