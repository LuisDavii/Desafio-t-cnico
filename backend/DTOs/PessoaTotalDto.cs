namespace ExpenseControl.Api.DTOs;

/// <summary>
/// Dados consolidados de totais de transações de uma pessoa.
/// </summary>
public class PessoaTotalDto
{
    /// <summary>
    /// Identificador único da pessoa.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa.
    /// </summary>
    public int Idade { get; set; }

    /// <summary>
    /// Soma de todas as receitas da pessoa.
    /// </summary>
    public decimal TotalReceitas { get; set; }

    /// <summary>
    /// Soma de todas as despesas da pessoa.
    /// </summary>
    public decimal TotalDespesas { get; set; }

    /// <summary>
    /// Saldo líquido da pessoa (Receitas - Despesas).
    /// </summary>
    public decimal Saldo { get; set; }
}
