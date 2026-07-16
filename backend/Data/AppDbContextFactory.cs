using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExpenseControl.Api.Data;

/// <summary>
/// Fábrica de contexto em tempo de design necessária para as migrações do Entity Framework Core.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Cria uma nova instância de <see cref="AppDbContext"/> para uso em tempo de design.
    /// </summary>
    /// <param name="args">Argumentos de linha de comando passados pelas ferramentas de migração.</param>
    /// <returns>Uma nova instância de <see cref="AppDbContext"/>.</returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=expense_control.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}
