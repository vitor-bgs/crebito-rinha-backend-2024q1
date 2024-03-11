using Crebito.Domain;

using Npgsql;
using System.Data;

namespace Crebito.Infra.DataAccess.WithDapper;

public sealed class UnitOfWorkDapper : IUnitOfWork
{
    private readonly NpgsqlDataSource _dataSource;
    private NpgsqlConnection? _connection = null;
    private NpgsqlTransaction? _transaction = null;

    public UnitOfWorkDapper(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    public async Task<IDbConnection> GetDbConnection()
    {
        if (_connection == null)
            _connection = await _dataSource.OpenConnectionAsync();
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        return _connection;
    }


    public async Task<IDbTransaction> BeginTransaction()
    {
        await GetDbConnection();
        if (_transaction == null)
        {
            _transaction = await _connection!.BeginTransactionAsync(IsolationLevel.RepeatableRead);
        }
        return _transaction;
    }

    public async Task Commit()
    {
        await _transaction!.CommitAsync();
        await DisposeItems();
    }

    public async Task Rollback()
    {
        await DisposeItems();
    }

    public async Task DisposeItems() 
    {

        if(_connection != null)
            await _connection!.CloseAsync();
        _transaction?.Dispose();
        _transaction = null;
        _connection?.Dispose();
        _connection = null;
    }

}
