using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseControl.Api.Models;

/// <summary>
/// Representa uma pessoa física registrada no sistema de controle de gastos.
/// </summary>
public class Pessoa
{
    /// <summary>
    /// Identificador único da pessoa.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Nome completo da pessoa.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa em anos.
    /// </summary>
    [Required]
    [Range(0, 150)]
    public int Idade { get; set; }

    /// <summary>
    /// Coleção de transações financeiras associadas a esta pessoa.
    /// </summary>
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
