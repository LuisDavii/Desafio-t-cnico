using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ExpenseControl.Api.Data;
using ExpenseControl.Api.Models;
using ExpenseControl.Api.Services;
using ExpenseControl.Api.Exceptions;

namespace ExpenseControl.Api;

/// <summary>
/// Executor de testes automatizados para validar as regras de negócio e persistência na Etapa 2.
/// </summary>
public static class TestRunner
{
    /// <summary>
    /// Executa todos os testes integrados utilizando um banco de dados SQLite em memória.
    /// </summary>
    public static async Task RunTestsAsync()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Iniciando testes da Etapa 2...");
        Console.WriteLine("========================================");

        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();

            var pessoaService = new PessoaService(context);
            var transacaoService = new TransacaoService(context);

            var adult = await pessoaService.CreateAsync(new Pessoa { Nome = "João Adulto", Idade = 25 });
            var teen = await pessoaService.CreateAsync(new Pessoa { Nome = "Maria Jovem", Idade = 15 });

            Console.WriteLine("Teste 1: Adicionar despesa para menor de idade (deve passar).");
            var transacaoTeenDespesa = await transacaoService.CreateAsync(new Transacao
            {
                Descricao = "Lanche",
                Valor = 25.50m,
                Tipo = TipoTransacao.Despesa,
                PersonId = teen.Id
            });
            Console.WriteLine($"[SUCESSO] Transação criada com ID: {transacaoTeenDespesa.Id}");

            Console.WriteLine("Teste 2: Adicionar receita para menor de idade (deve falhar).");
            try
            {
                await transacaoService.CreateAsync(new Transacao
                {
                    Descricao = "Mesada",
                    Valor = 100.00m,
                    Tipo = TipoTransacao.Receita,
                    PersonId = teen.Id
                });
                Console.WriteLine("[FALHA] Foi possível criar receita para menor de idade.");
            }
            catch (BusinessException ex)
            {
                Console.WriteLine($"[SUCESSO] Erro esperado capturado: {ex.Message}");
            }

            Console.WriteLine("Teste 3: Adicionar receita e despesa para maior de idade (deve passar).");
            var transacaoAdultReceita = await transacaoService.CreateAsync(new Transacao
            {
                Descricao = "Salário",
                Valor = 5000.00m,
                Tipo = TipoTransacao.Receita,
                PersonId = adult.Id
            });
            var transacaoAdultDespesa = await transacaoService.CreateAsync(new Transacao
            {
                Descricao = "Aluguel",
                Valor = 1500.00m,
                Tipo = TipoTransacao.Despesa,
                PersonId = adult.Id
            });
            Console.WriteLine($"[SUCESSO] Transações criadas para adulto. Receita ID: {transacaoAdultReceita.Id}, Despesa ID: {transacaoAdultDespesa.Id}");

            Console.WriteLine("Teste 4: Verificar totais calculados.");
            var totals = await pessoaService.GetTotalsAsync();
            var teenTotal = totals.Pessoas.First(p => p.Id == teen.Id);
            var adultTotal = totals.Pessoas.First(p => p.Id == adult.Id);

            if (teenTotal.TotalReceitas == 0 && teenTotal.TotalDespesas == 25.50m && teenTotal.Saldo == -25.50m)
            {
                Console.WriteLine("[SUCESSO] Totais da Maria calculados corretamente.");
            }
            else
            {
                Console.WriteLine($"[FALHA] Totais da Maria incorretos. Receitas: {teenTotal.TotalReceitas}, Despesas: {teenTotal.TotalDespesas}, Saldo: {teenTotal.Saldo}");
            }

            if (adultTotal.TotalReceitas == 5000.00m && adultTotal.TotalDespesas == 1500.00m && adultTotal.Saldo == 3500.00m)
            {
                Console.WriteLine("[SUCESSO] Totais do João calculados corretamente.");
            }
            else
            {
                Console.WriteLine($"[FALHA] Totais do João incorretos. Receitas: {adultTotal.TotalReceitas}, Despesas: {adultTotal.TotalDespesas}, Saldo: {adultTotal.Saldo}");
            }

            if (totals.TotalGeralReceitas == 5000.00m && totals.TotalGeralDespesas == 1525.50m && totals.SaldoGeral == 3474.50m)
            {
                Console.WriteLine("[SUCESSO] Totais gerais calculados corretamente.");
            }
            else
            {
                Console.WriteLine($"[FALHA] Totais gerais incorretos. Receitas: {totals.TotalGeralReceitas}, Despesas: {totals.TotalGeralDespesas}, Saldo: {totals.SaldoGeral}");
            }

            Console.WriteLine("Teste 5: Exclusão em cascata (ao deletar pessoa, deletar transações).");
            var transacoesAntes = await context.Transacoes.CountAsync();
            Console.WriteLine($"Transações no banco antes da exclusão: {transacoesAntes}");

            var deletado = await pessoaService.DeleteAsync(adult.Id);
            if (deletado)
            {
                var transacoesDepois = await context.Transacoes.CountAsync();
                Console.WriteLine($"Transações no banco após exclusão do adulto: {transacoesDepois}");
                if (transacoesDepois == 1)
                {
                    Console.WriteLine("[SUCESSO] Exclusão em cascata funcionou perfeitamente.");
                }
                else
                {
                    Console.WriteLine($"[FALHA] Esperava-se 1 transação, mas foram encontradas: {transacoesDepois}");
                }
            }
            else
            {
                Console.WriteLine("[FALHA] Não foi possível deletar o adulto.");
            }
        }

        Console.WriteLine("========================================");
        Console.WriteLine("Fim dos testes.");
        Console.WriteLine("========================================");
    }
}
