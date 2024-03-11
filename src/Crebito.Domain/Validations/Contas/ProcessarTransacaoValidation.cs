using Crebito.Domain.Models.Contas;
using FluentValidation;

namespace Crebito.Domain.Validations.Contas;

internal class ProcessarTransacaoValidation : AbstractValidator<Conta>
{
    public ProcessarTransacaoValidation()
    {
        RuleFor(x => x.Saldo)
            .GreaterThanOrEqualTo(x => x.Limite * (-1))
            .WithMessage("saldo insuficiente");
    }
}
