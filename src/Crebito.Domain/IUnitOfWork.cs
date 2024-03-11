using System.Data;

namespace Crebito.Domain;

public interface IUnitOfWork
{
    Task<IDbConnection> GetDbConnection();
    Task<IDbTransaction> BeginTransaction();
    Task Commit();
    Task Rollback();
}
