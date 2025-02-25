using System.Data.Common;

namespace QwackX.Api.Domain.Services;

public abstract class BaseService
{
    public DbConnection DbConnection { get; }

    protected BaseService(DbConnection dbConnection)
    {
        DbConnection = dbConnection;
        DbConnection.Open();
    }
}
