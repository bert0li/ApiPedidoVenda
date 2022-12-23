using System;
using ApiPedidoVenda.Data;
using ApiPedidoVenda.Extensions;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.Util;
using ApiPedidoVenda.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidoVenda.Controllers
{
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpGet("v1/clientes")]
        public async Task<IActionResult> ObterTodosAsync([FromServices] ContextoPedidoVenda contexto)
        {
            try
            {
                var clientes = await contexto.Clientes.ToListAsync();

                return Ok(new ResultadoViewModel<IEnumerable<Cliente>>(clientes));
            }
            catch (Exception ex)
            {
                LogUtil.LogExceptionController(ex, "ClienteController", "ObterTodosAsync");
                return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
            }
        }

        [HttpGet("v1/clientes/{id:int}")]
        public async Task<IActionResult> ObterPorIdAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id)
        {
            try
            {
                var cliente = await contexto.Clientes.FirstOrDefaultAsync(f => f.Id == id);

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
        public async Task<IActionResult> IncluirAsync([FromServices] ContextoPedidoVenda contexto, [FromBody] ClienteViewModel model)
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

                await contexto.Clientes.AddAsync(cliente);
                await contexto.SaveChangesAsync();

                return Created($"v1/clientes/{cliente.Id}", new ResultadoViewModel<Cliente>(cliente));
            }
            catch (Exception ex)
            {
                LogUtil.LogExceptionController(ex, "ClienteController", "IncluirAsync");
                return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
            }
        }

        [HttpPut("v1/clientes/{id:int}")]
        public async Task<IActionResult> AtualizarAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id, [FromBody] ClienteViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultadoViewModel<Cliente>(ModelState.ObterErrosModelState()));

                var cliente = await contexto.Clientes.FirstOrDefaultAsync(f => f.Id == id);

                if (cliente == null)
                    return BadRequest(new ResultadoViewModel<string>($"Cliente com código: [{id}] não encontrado."));

                cliente.Nome = model.Nome;
                cliente.Email = model.Email;

                contexto.Clientes.Update(cliente);
                await contexto.SaveChangesAsync();

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
}