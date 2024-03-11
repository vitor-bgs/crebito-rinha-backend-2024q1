using Crebito.Common.ErrorNotifications;
using Crebito.Domain.Models.Transacoes;
using Crebito.Domain.Services.Dtos;
using FluentValidation;

namespace Crebito.Domain.Validations.Requests
{
    internal class ProcessarTransacaoRequestValidation : AbstractValidator<ProcessarTransacaoRequest>
    {
        private readonly char[] tiposValidos = { TipoTransacao.Credito, TipoTransacao.Debito };
        
        public ProcessarTransacaoRequestValidation()
        {

            RuleFor(x => x.descricao)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(10);

            RuleFor(x => x.tipo)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(1)
                .Must(x => tiposValidos.Contains(x[0]));

            RuleFor(x => x.valor)
                .GreaterThanOrEqualTo(0);

        }
    }
}
