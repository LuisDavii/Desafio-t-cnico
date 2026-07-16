using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseControl.Api.Data;
using ExpenseControl.Api.Models;
using ExpenseControl.Api.Exceptions;

namespace ExpenseControl.Api.Services;

/// <summary>
/// Implementação do serviço de transações financeiras, aplicando as regras de validação necessárias.
/// </summary>
public class TransacaoService : ITransacaoService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TransacaoService"/> com o contexto do banco de dados.
    /// </summary>
    /// <param name="context">O contexto do banco de dados.</param>
    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cria uma nova transação financeira, validando se a pessoa associada tem menos de 18 anos e está tentando registrar uma receita.
    /// </summary>
    /// <param name="transacao">A transação a ser salva.</param>
    /// <returns>A transação criada.</returns>
    /// <exception cref="BusinessException">Lançada caso a pessoa não seja encontrada ou seja menor de idade tentando registrar receita.</exception>
    public async Task<Transacao> CreateAsync(Transacao transacao)
    {
        var pessoa = await _context.Pessoas.FindAsync(transacao.PersonId);
        if (pessoa == null)
        {
            throw new BusinessException("Pessoa não encontrada.");
        }

        if (pessoa.Idade < 18 && transacao.Tipo == TipoTransacao.Receita)
        {
            throw new BusinessException("Pessoas menores de 18 anos só podem registrar Despesas.");
        }

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        return transacao;
    }

    /// <summary>
    /// Obtém todas as transações cadastradas, incluindo as informações da pessoa associada.
    /// </summary>
    /// <returns>Coleção de transações.</returns>
    public async Task<IEnumerable<Transacao>> GetAllAsync()
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .ToListAsync();
    }
}
