using Crebito.Common.ErrorNotifications;
using Crebito.Domain.Queries;
using Crebito.Domain.Services.Dtos;
using Dapper;
using Npgsql;

namespace Crebito.Infra.DataAccess.WithDapper.Queries;

public class ObterExtratoQueryServiceDapper : IObterExtratoQueryService
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly ErrorNotificationService _notificationService;

    public ObterExtratoQueryServiceDapper(NpgsqlDataSource datasource, ErrorNotificationService notificationService)
    {
        _dataSource = datasource;
        _notificationService = notificationService;
    }

    public async Task<ObterExtratoResponse?> ObterExtrato(int clienteId)
    {
        using var connection = await _dataSource.OpenConnectionAsync();

        const string sql = """
                                select saldo Total, limite Limite, timezone('utc', now()) data_extrato
                                from contas where contas.cliente_id = @ClienteId;

                                select valor, tipo, descricao, realizada_em
                                from transacoes 
                                where conta_id = (select id from contas where contas.cliente_id = @ClienteId) 
                                order by realizada_em desc 
                                limit 10;
                            """;

        try
        {
            using (var multi = await connection.QueryMultipleAsync(sql, new { ClienteId = clienteId }))
            {
                ObterExtratoResponseSaldo? saldo = multi.Read<ObterExtratoResponseSaldo>().FirstOrDefault();
                if (saldo == null)
                {
                    _notificationService.AddError(NotificationErrorType.NotFound, "Conta não existe");
                    return null;
                }

                IList<ObterExtratoResponseTransacao>? transacoes = multi.Read<ObterExtratoResponseTransacao>()?.ToList();

                return new ObterExtratoResponse(saldo, transacoes);
            };
        }
        finally
        {
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
    }
}
