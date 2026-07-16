using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpenseControl.Api.Models;
using ExpenseControl.Api.Services;
using ExpenseControl.Api.Exceptions;

namespace ExpenseControl.Api.Controllers;

/// <summary>
/// Controlador responsável pelos endpoints da entidade Transação.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TransacoesController"/> com o serviço injetado.
    /// </summary>
    /// <param name="transacaoService">O serviço de transações financeiras.</param>
    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    /// <summary>
    /// Retorna a listagem completa de todas as transações do sistema.
    /// </summary>
    /// <returns>Uma lista contendo todas as transações financeiras.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transacao>>> GetAll()
    {
        var transacoes = await _transacaoService.GetAllAsync();
        return Ok(transacoes);
    }

    /// <summary>
    /// Registra uma nova transação financeira, validando a restrição de idade da pessoa associada.
    /// </summary>
    /// <param name="transacao">Os dados da transação a ser criada.</param>
    /// <returns>A transação criada, ou HTTP 400 Bad Request se a regra de negócio for violada.</returns>
    [HttpPost]
    public async Task<ActionResult<Transacao>> Create(Transacao transacao)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _transacaoService.CreateAsync(transacao);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
