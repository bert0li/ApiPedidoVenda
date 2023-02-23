using System;
using ApiPedidoVenda.Data;
using ApiPedidoVenda.Extensions;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.Repositorio;
using ApiPedidoVenda.Repositorio.Base;
using ApiPedidoVenda.Util;
using ApiPedidoVenda.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidoVenda.Controllers;

[ApiController]
public class ClienteController : ControllerBase
{
    ///.../v1/clientes?numeroDaPagina=1&itensPorPagina=10
    [HttpGet("v1/clientes")]
    public async Task<IActionResult> ObterTodosAsync([FromServices] IRepositorio<Cliente> repositorio, [FromQuery] int numeroDaPagina = 0, [FromQuery] int itensPorPagina = 20)
    {
        try
        {
            var clientes =  repositorio.ObterTodosAsync(numeroDaPagina, itensPorPagina);

            return Ok(new ResultadoViewModel<IEnumerable<Cliente>>(clientes));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "ClienteController", "ObterTodosAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpGet("v1/clientes/{id:int}")]
    public async Task<IActionResult> ObterPorIdAsync([FromServices] IRepositorio<Cliente> repositorio, [FromRoute] int id)
    {
        try
        {
            var cliente = repositorio.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound(new ResultadoViewModel<string>($"Cliente com código: [{id}] não encontrado."));

            return Ok(new ResultadoViewModel<Cliente>(cliente));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "ClienteController", "ObterPorIdAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpPost("v1/clientes")]
    public async Task<IActionResult> IncluirAsync([FromServices] IRepositorio<Cliente> repositorio, [FromBody] ClienteViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultadoViewModel<string>(ModelState.ObterErrosModelState()));

            var cliente = new Cliente()
            {
                Id = 0,
                Nome = model.Nome,
                Email = model.Email
            };

            repositorio.IncluirAsync(cliente);

            return Created($"v1/clientes/{cliente.Id}", new ResultadoViewModel<Cliente>(cliente));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "ClienteController", "IncluirAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpPut("v1/clientes/{id:int}")]
    public async Task<IActionResult> AtualizarAsync([FromServices] IRepositorio<Cliente> repositorio, [FromRoute] int id, [FromBody] ClienteViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultadoViewModel<Cliente>(ModelState.ObterErrosModelState()));

            var cliente = repositorio.ObterPorIdAsync(id);

            if (cliente == null)
                return BadRequest(new ResultadoViewModel<string>($"Cliente com código: [{id}] não encontrado."));

            cliente.Nome = model.Nome;
            cliente.Email = model.Email;


            return Ok(new ResultadoViewModel<Cliente>(cliente));
        }
        catch (DbUpdateException updateException)
        {
            LogUtil.LogExceptionController(updateException, "ClienteController", "AtualizarAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Não foi possível alterar o cliente."));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "ClienteController", "AtualizarAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/clientes/{id:int}")]
    public async Task<IActionResult> DeletarAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id)
    {
        try
        {
            var cliente = await contexto.Clientes.FirstOrDefaultAsync(f => f.Id == id);

            if (cliente == null)
                return BadRequest(new ResultadoViewModel<string>($"Cliente com código: [{id}] não encontrado."));

            contexto.Clientes.Remove(cliente);
            await contexto.SaveChangesAsync();

            return Ok(new ResultadoViewModel<Cliente>(cliente));
        }
        catch (DbUpdateException updateException)
        {
            LogUtil.LogExceptionController(updateException, "ClienteController", "DeletarAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Não foi possível excluir o cliente."));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "ClienteController", "DeletarAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }
}