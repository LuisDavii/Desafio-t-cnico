namespace ExpenseControl.Api.Models;

/// <summary>
/// Define os tipos de transações financeiras suportados pelo sistema.
/// </summary>
public enum TipoTransacao
{
    /// <summary>
    /// Representa uma receita (entrada de dinheiro).
    /// </summary>
    Receita,

    /// <summary>
    /// Representa uma despesa (saída de dinheiro).
    /// </summary>
    Despesa
}
