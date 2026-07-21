namespace Inventory.Application.Abstractions.Authentication
{
    public interface IJwtConfiguration
    {
        JwtOptions Current { get; }
    }
}