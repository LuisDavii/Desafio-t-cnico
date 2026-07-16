using ExpenseControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Api.Data;

/// <summary>
/// Contexto do banco de dados da aplicação, responsável por gerenciar a conexão e o mapeamento das entidades.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="AppDbContext"/> com as opções fornecidas.
    /// </summary>
    /// <param name="options">As opções do contexto do banco de dados.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AppDbContext"/> com construtor padrão.
    /// </summary>
    public AppDbContext()
    {
    }

    /// <summary>
    /// Conjunto de dados correspondente às pessoas no banco de dados.
    /// </summary>
    public DbSet<Pessoa> Pessoas { get; set; } = null!;

    /// <summary>
    /// Conjunto de dados correspondente às transações no banco de dados.
    /// </summary>
    public DbSet<Transacao> Transacoes { get; set; } = null!;

    /// <summary>
    /// Configura o banco de dados SQLite caso nenhuma configuração prévia tenha sido definida.
    /// </summary>
    /// <param name="optionsBuilder">O construtor das opções de configuração do contexto.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=expense_control.db");
        }
    }

    /// <summary>
    /// Configura os relacionamentos, chaves primárias e comportamento de exclusão em cascata.
    /// </summary>
    /// <param name="modelBuilder">O construtor de modelos para configuração do banco de dados.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Idade).IsRequired();

            entity.HasMany(e => e.Transacoes)
                  .WithOne(t => t.Pessoa)
                  .HasForeignKey(t => t.PersonId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tipo).IsRequired();
            entity.Property(e => e.PersonId).IsRequired();
        });
    }
}
