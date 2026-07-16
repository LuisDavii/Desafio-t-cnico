using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseControl.Api.Models;
using ExpenseControl.Api.DTOs;

namespace ExpenseControl.Api.Services;

/// <summary>
/// Contrato para o serviço de gerenciamento de pessoas.
/// </summary>
public interface IPessoaService
{
    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// </summary>
    /// <param name="pessoa">A entidade pessoa com os dados a serem criados.</param>
    /// <returns>A entidade pessoa recém-criada com seu identificador preenchido.</returns>
    Task<Pessoa> CreateAsync(Pessoa pessoa);

    /// <summary>
    /// Exclui uma pessoa do sistema. Devido à integridade referencial configurada, as transações associadas serão excluídas em cascata.
    /// </summary>
    /// <param name="id">O identificador da pessoa a ser excluída.</param>
    /// <returns>True se a exclusão foi bem-sucedida; caso contrário, false.</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Retorna a listagem de todas as pessoas cadastradas.
    /// </summary>
    /// <returns>Uma lista contendo todas as pessoas.</returns>
    Task<IEnumerable<Pessoa>> GetAllAsync();

    /// <summary>
    /// Obtém a listagem consolidadas de totais por pessoa, além dos totais e saldos gerais.
    /// </summary>
    /// <returns>Um DTO contendo o demonstrativo de totais individuais e totais gerais.</returns>
    Task<DashboardTotalsDto> GetTotalsAsync();
}
