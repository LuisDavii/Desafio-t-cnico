using System.Collections.Generic;

namespace ExpenseControl.Api.DTOs;

/// <summary>
/// Dados consolidados de todas as pessoas e totais gerais do sistema.
/// </summary>
public class DashboardTotalsDto
{
    /// <summary>
    /// Lista de totais individuais por pessoa.
    /// </summary>
    public List<PessoaTotalDto> Pessoas { get; set; } = new();

    /// <summary>
    /// Soma das receitas de todas as pessoas cadastradas.
    /// </summary>
    public decimal TotalGeralReceitas { get; set; }

    /// <summary>
    /// Soma das despesas de todas as pessoas cadastradas.
    /// </summary>
    public decimal TotalGeralDespesas { get; set; }

    /// <summary>
    /// Saldo acumulado geral de todas as pessoas.
    /// </summary>
    public decimal SaldoGeral { get; set; }
}
