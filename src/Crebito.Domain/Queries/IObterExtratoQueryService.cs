using Crebito.Domain.Services.Dtos;

namespace Crebito.Domain.Queries;

public interface IObterExtratoQueryService
{
    Task<ObterExtratoResponse?> ObterExtrato(int clienteId);
}
