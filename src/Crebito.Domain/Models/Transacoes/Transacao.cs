namespace Crebito.Domain.Models.Transacoes;

public class Transacao
{
    public Transacao(int contaId, char tipo, int valor, string descricao)
    {
        ContaId = contaId;
        Tipo = tipo;
        Valor = valor;
        Descricao = descricao;
        RealizadaEm = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public int ContaId { get; set; }
    public char Tipo { get; set; }
    public string Descricao { get; set; }
    public int Valor { get; set; }
    public DateTime RealizadaEm { get; set; }
}

