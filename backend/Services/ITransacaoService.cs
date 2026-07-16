using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Services;

/// <summary>
/// Contrato para o serviço de gerenciamento de transações financeiras.
/// </summary>
public interface ITransacaoService
{
    /// <summary>
    /// Cria e registra uma nova transação financeira vinculada a uma pessoa.
    /// Valida as regras de negócio referentes à idade da pessoa e ao tipo de transação permitido.
    /// </summary>
    /// <param name="transacao">A entidade transação com os dados informados.</param>
    /// <returns>A entidade transação cadastrada.</returns>
    /// <exception cref="Exceptions.BusinessException">Lançada se a regra de negócio da idade mínima para receitas for violada.</exception>
    Task<Transacao> CreateAsync(Transacao transacao);

    /// <summary>
    /// Retorna a listagem de todas as transações cadastradas.
    /// </summary>
    /// <returns>Uma lista contendo todas as transações.</returns>
    Task<IEnumerable<Transacao>> GetAllAsync();
}
