using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseControl.Api.Data;
using ExpenseControl.Api.Models;
using ExpenseControl.Api.DTOs;

namespace ExpenseControl.Api.Services;

/// <summary>
/// Implementação do serviço de gerenciamento de pessoas com persistência via Entity Framework Core.
/// </summary>
public class PessoaService : IPessoaService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Construtor que inicializa o serviço com o contexto do banco de dados.
    /// </summary>
    /// <param name="context">O contexto do banco de dados a ser injetado.</param>
    public PessoaService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cria uma nova pessoa no banco de dados.
    /// </summary>
    /// <param name="pessoa">A entidade pessoa com os dados informados.</param>
    /// <returns>A entidade cadastrada.</returns>
    public async Task<Pessoa> CreateAsync(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        return pessoa;
    }

    /// <summary>
    /// Exclui uma pessoa do banco de dados. Exclui em cascata todas as transações referenciadas pelo ID da pessoa.
    /// </summary>
    /// <param name="id">O identificador da pessoa.</param>
    /// <returns>True se removido com sucesso; caso contrário, false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null)
        {
            return false;
        }

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retorna uma lista de todas as pessoas salvas na base de dados.
    /// </summary>
    /// <returns>Coleção de pessoas.</returns>
    public async Task<IEnumerable<Pessoa>> GetAllAsync()
    {
        return await _context.Pessoas.ToListAsync();
    }

    /// <summary>
    /// Obtém um demonstrativo financeiro consolidado contendo os totais de receitas, despesas e saldo individualizado por pessoa e geral.
    /// </summary>
    /// <returns>Um DTO contendo a lista consolidada e os acumuladores globais.</returns>
    public async Task<DashboardTotalsDto> GetTotalsAsync()
    {
        var pessoas = await _context.Pessoas
            .Include(p => p.Transacoes)
            .ToListAsync();

        var listPessoasDto = pessoas.Select(p =>
        {
            var totalReceitas = p.Transacoes
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalDespesas = p.Transacoes
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            return new PessoaTotalDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas
            };
        }).ToList();

        var totalGeralReceitas = listPessoasDto.Sum(p => p.TotalReceitas);
        var totalGeralDespesas = listPessoasDto.Sum(p => p.TotalDespesas);

        return new DashboardTotalsDto
        {
            Pessoas = listPessoasDto,
            TotalGeralReceitas = totalGeralReceitas,
            TotalGeralDespesas = totalGeralDespesas,
            SaldoGeral = totalGeralReceitas - totalGeralDespesas
        };
    }
}
