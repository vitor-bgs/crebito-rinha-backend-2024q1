namespace Crebito.Domain.Services.Dtos;

public record ObterExtratoResponse(
    ObterExtratoResponseSaldo saldo, 
    IList<ObterExtratoResponseTransacao?>? ultimas_transacoes
);

public record ObterExtratoResponseSaldo(
    int total,
    int Limite,
    DateTime data_extrato
);

public record ObterExtratoResponseTransacao(
    int valor, 
    char tipo, 
    string descricao, 
    DateTime realizada_em
);
