
using Crebito.Domain.Models.Transacoes;
using Crebito.Domain.Validations.Contas;
using FluentValidation.Results;

namespace Crebito.Domain.Models.Contas;

public class Conta
{
    public int Id { get; protected set; }
    public int ClienteId { get; protected set; }
    public int Limite { get; protected set; }
    public int Saldo { get; protected set; }

    public ValidationResult ProcessarTransacao(Transacao transacao)
    {
        switch (transacao.Tipo)
        {
            case TipoTransacao.Credito: 
                CreditarSaldo(transacao.Valor);
                break;
            case TipoTransacao.Debito:
                DebitarSaldo(transacao.Valor);
                break;
            default: 
                throw new ArgumentOutOfRangeException(nameof(transacao.Tipo));
        }

        var validation = new ProcessarTransacaoValidation().Validate(this);
        return validation;
    }

    public void CreditarSaldo(int valor)
    {
        Saldo += valor;
    }

    public void DebitarSaldo(int valor)
    {
        Saldo -= valor;
    }
}
