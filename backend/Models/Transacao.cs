using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseControl.Api.Models;

/// <summary>
/// Representa uma transação financeira (receita ou despesa) associada a uma pessoa.
/// </summary>
public class Transacao
{
    /// <summary>
    /// Identificador único da transação.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Descrição da transação.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor monetário da transação.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    /// <summary>
    /// Tipo da transação, indicando se é uma receita ou despesa.
    /// </summary>
    [Required]
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// Identificador da pessoa proprietária da transação.
    /// </summary>
    [Required]
    public int PersonId { get; set; }

    /// <summary>
    /// Propriedade de navegação para a pessoa associada a esta transação.
    /// </summary>
    [ForeignKey(nameof(PersonId))]
    public Pessoa? Pessoa { get; set; }
}
