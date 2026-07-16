using System;

namespace ExpenseControl.Api.Exceptions;

/// <summary>
/// Exceção lançada quando uma regra de negócio do sistema de controle de despesas é violada.
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="BusinessException"/> com uma mensagem de erro específica.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro de regra de negócio.</param>
    public BusinessException(string message) : base(message)
    {
    }
}
