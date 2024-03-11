using Crebito.Domain;
using Crebito.Domain.Models.Contas;
using Crebito.Domain.Models.Transacoes;
using Crebito.Domain.Repository;

using Dapper;

namespace Crebito.Infra.DataAccess.WithDapper.Repository;

public class ContaRepositoryDapper : IContaRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ContaRepositoryDapper(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Conta?> ObterContaPorClienteId(int clienteId)
    {
        var connection = await _unitOfWork.GetDbConnection();
        var dbTransaction = await _unitOfWork.BeginTransaction();

        const string sql = @"select id, limite, saldo from contas where cliente_id = @ClienteId;";

        var conta = await connection.QuerySingleOrDefaultAsync<Conta?>(sql, new { ClienteId = clienteId }, dbTransaction);
        return conta;
    }

    public async Task RegistrarTransacaoAsync(Transacao transacao, Conta conta)
    {
        var connection = await _unitOfWork.GetDbConnection();
        var dbTransaction = await _unitOfWork.BeginTransaction();

        const string sql =
            """
            UPDATE contas SET saldo = @Saldo WHERE id = @ContaId;
            INSERT INTO transacoes (conta_id, valor, descricao, tipo, realizada_em)
            VALUES (@ContaId, @Valor, @Descricao, @Tipo, @RealizadaEm);
            """;

        try
        {
            await connection.ExecuteAsync(
                sql, 
                new {
                    conta.Saldo,
                    transacao.ContaId,
                    transacao.Valor,
                    transacao.Descricao,
                    transacao.Tipo,
                    transacao.RealizadaEm
                }, 
                dbTransaction
            );
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
