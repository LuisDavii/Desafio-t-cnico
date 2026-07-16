using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpenseControl.Api.Models;
using ExpenseControl.Api.Services;
using ExpenseControl.Api.DTOs;

namespace ExpenseControl.Api.Controllers;

/// <summary>
/// Controlador responsável pelos endpoints da entidade Pessoa e relatórios de totais.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="PessoasController"/> com o serviço injetado.
    /// </summary>
    /// <param name="pessoaService">O serviço de gerenciamento de pessoas.</param>
    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    /// <summary>
    /// Obtém a listagem simples de todas as pessoas.
    /// </summary>
    /// <returns>Uma lista contendo todas as pessoas.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetAll()
    {
        var pessoas = await _pessoaService.GetAllAsync();
        return Ok(pessoas);
    }

    /// <summary>
    /// Obtém a consolidação financeira (totais por pessoa e totais gerais) para o dashboard.
    /// </summary>
    /// <returns>Os dados financeiros de totais e saldos.</returns>
    [HttpGet("totais")]
    public async Task<ActionResult<DashboardTotalsDto>> GetTotals()
    {
        var totals = await _pessoaService.GetTotalsAsync();
        return Ok(totals);
    }

    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// </summary>
    /// <param name="pessoa">Os dados da pessoa a ser cadastrada.</param>
    /// <returns>A pessoa cadastrada com sua rota correspondente.</returns>
    [HttpPost]
    public async Task<ActionResult<Pessoa>> Create(Pessoa pessoa)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _pessoaService.CreateAsync(pessoa);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove uma pessoa e suas respectivas transações financeiras em cascata.
    /// </summary>
    /// <param name="id">O identificador da pessoa.</param>
    /// <returns>Status de NoContent se removida com sucesso, ou NotFound caso contrário.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _pessoaService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
