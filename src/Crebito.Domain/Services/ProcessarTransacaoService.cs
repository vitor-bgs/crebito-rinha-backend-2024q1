using Crebito.Common.ErrorNotifications;
using Crebito.Domain.Models.Transacoes;
using Crebito.Domain.Repository;
using Crebito.Domain.Services.Dtos;
using Crebito.Domain.Validations.Requests;

namespace Crebito.Domain.Services;
public class ProcessarTransacaoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContaRepository _clienteRepository;
    private readonly ErrorNotificationService _notificationService;

    public ProcessarTransacaoService(
        IUnitOfWork unitOfWork,
        IContaRepository clienteRepository,
        ErrorNotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _clienteRepository = clienteRepository;
        _notificationService = notificationService;
    }

    public async Task<ProcessarTransacaoResponse?> ProcessarTransacao(int clienteId, ProcessarTransacaoRequest request)
    {
        if (request is null)
        {
            _notificationService.AddError(NotificationErrorType.Domain, "Requisição inválida");
            return null;
        }

        var requestValidation = new ProcessarTransacaoRequestValidation().Validate(request);
        if(!requestValidation.IsValid)
        {
            _notificationService.AddError(NotificationErrorType.Domain, string.Join(", ", requestValidation.Errors));
            return null;
        }


        using var transaction = await _unitOfWork.BeginTransaction();

        var conta = await _clienteRepository.ObterContaPorClienteId(clienteId);
        if (conta is null)
        {
            _notificationService.AddError(NotificationErrorType.NotFound, "Conta não encontrado");
            await _unitOfWork.Rollback();
            return null;
        }

        var transacao = new Transacao(clienteId, request.tipo![0], request.valor!.Value, request.descricao!);
        var processarTransacaoValidation = conta.ProcessarTransacao(transacao);
        if (!processarTransacaoValidation.IsValid)
        {
            _notificationService.AddError(NotificationErrorType.Domain, string.Join(", ", processarTransacaoValidation.Errors));
            await _unitOfWork.Rollback();
            return null;
        }

        await _clienteRepository.RegistrarTransacaoAsync(transacao, conta);
        await _unitOfWork.Commit();

        return new ProcessarTransacaoResponse(conta.Limite, conta.Saldo);

    }
}
