using Crebito.Domain.Models.Contas;
using Crebito.Domain.Models.Transacoes;

namespace Crebito.Domain.Repository;

public interface IContaRepository
{
    Task<Conta?> ObterContaPorClienteId(int clienteId);
    Task RegistrarTransacaoAsync(Transacao transacao, Conta conta);
}
