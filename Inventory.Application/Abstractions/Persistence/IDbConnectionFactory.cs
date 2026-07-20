using System.Data;

namespace Inventory.Application.Abstractions.Persistence
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}