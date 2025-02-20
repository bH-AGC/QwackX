using System.Data.Common;

namespace QwackX.Api.Domain.Repositories;

public abstract class BaseRepository
{
    public DbConnection DbConnection { get; }

    protected BaseRepository(DbConnection dbConnection)
    {
        DbConnection = dbConnection;
        DbConnection.Open();
    }
}
