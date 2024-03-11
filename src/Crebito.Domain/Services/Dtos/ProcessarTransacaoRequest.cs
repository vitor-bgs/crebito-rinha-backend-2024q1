namespace Crebito.Domain.Services.Dtos;
public record ProcessarTransacaoRequest(
    int? valor, 
    string? tipo, 
    string? descricao);

