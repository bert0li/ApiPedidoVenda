using ApiPedidoVenda.Data;
using ApiPedidoVenda.Enum;
using ApiPedidoVenda.Extensions;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.Util;
using ApiPedidoVenda.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidoVenda.Controllers;

[ApiController]
public class PedidoController : ControllerBase
{
    ///.../v1/pedidos?numeroDaPagina=1&itensPorPagina=10
    [HttpGet("v1/pedidos")]
    public async Task<IActionResult> ObterTodosAsync([FromServices] ContextoPedidoVenda contexto, [FromQuery] int numeroDaPagina = 0, [FromQuery] int itensPorPagina = 20)
    {
        try
        {
            var pedidos = await contexto.Pedidos
                                        .AsNoTracking()
                                        .Include(i => i.Cliente)
                                        .Include(i => i.Itens)
                                        .Skip(numeroDaPagina * itensPorPagina)
                                        .Take(itensPorPagina)
                                        .ToListAsync();

            return Ok(new ResultadoViewModel<IEnumerable<Pedido>>(pedidos));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "PedidoController", "ObterTodosAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor."));
        }
    }

    [HttpGet("v1/pedidos/{id:int}")]
    public async Task<IActionResult> ObterPorIdAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id)
    {
        try
        {
            var pedido = await contexto.Pedidos
                                       .Include(i => i.Cliente)
                                       .Include(i => i.Itens)
                                       .FirstOrDefaultAsync(f => f.Id == id);

            if (pedido == null)
                return BadRequest(new ResultadoViewModel<Pedido>($"Pedido com o código: [{id}] não encontrado."));

            return Ok(new ResultadoViewModel<Pedido>(pedido));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "PedidoController", "ObterPorIdAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpPost("v1/pedidos")]
    public async Task<IActionResult> IncluirAsync([FromServices] ContextoPedidoVenda contexto, [FromBody] PedidoViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultadoViewModel<string>(ModelState.ObterErrosModelState()));

            if (model.Itens == null || model.Itens.Count() == 0)
                return BadRequest(new ResultadoViewModel<string>("Pedido não tem itens."));

            var pedido = new Pedido()
            {
                Id = 0,
                DataPedido = model.DataPedido,
                ClienteId = model?.Cliente?.Id,
            };

            foreach (var item in model.Itens)
            {
                var produto = await contexto.Produtos.FirstOrDefaultAsync(f => f.Id == item.Id);

                if (produto != null)
                    pedido.AddItem(produto);
            }

            await contexto.Pedidos.AddAsync(pedido);
            await contexto.SaveChangesAsync();

            return Created($"v1/pedidos/{pedido.Id}", pedido);
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "PedidoController", "IncluirAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpPut("v1/pedidos/cancelar/{id:int}")]
    public async Task<IActionResult> CancelarAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id)
    {
        try
        {
            var pedido = await contexto.Pedidos.FirstOrDefaultAsync(f => f.Id == id);

            if (pedido == null)
                return BadRequest(new ResultadoViewModel<string>($"Pedido de número: [{id}] não encontrado."));

            if (pedido.Cancelado)
                return BadRequest(new ResultadoViewModel<string>($"Pedido de número: [{pedido.Id}] já esta cancelado"));

            pedido.Cancelado = true;
            contexto.Pedidos.Update(pedido);
            await contexto.SaveChangesAsync();

            return Ok(new ResultadoViewModel<Pedido>(pedido));
        }
        catch (DbUpdateException updateException)
        {
            LogUtil.LogExceptionController(updateException, "PedidoController", "CancelarAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Não foi possível cancelar o pedido."));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "PedidoController", "CancelarAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpPut("v1/pedidos/transformar/{id:int}")]
    public async Task<IActionResult> TransformarEmPedidoAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id)
    {
        try
        {
            var pedido = await contexto.Pedidos.FirstOrDefaultAsync(f => f.Id == id);

            if (pedido == null)
                return BadRequest(new ResultadoViewModel<string>($"Pedido de número: [{id}] não encontrado."));

            if (pedido.TipoPedido == TipoPedido.Pedido)
                return BadRequest(new ResultadoViewModel<string>($"Pedido de número: [{pedido.Id}] já é um Pedido."));

            pedido.TipoPedido = TipoPedido.Pedido;
            contexto.Pedidos.Update(pedido);
            await contexto.SaveChangesAsync();

            return Ok(new ResultadoViewModel<Pedido>(pedido));
        }
        catch (DbUpdateException updateException)
        {
            LogUtil.LogExceptionController(updateException, "PedidoController", "TransformarEmPedidoAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Não foi possível cancelar o pedido."));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "PedidoController", "TransformarEmPedidoAsync");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }

    [HttpGet("v1/pedidos/pesquisar/")]
    public async Task<IActionResult> ObterPedidoPorPeriodo([FromServices] ContextoPedidoVenda contexto, [FromBody] PedidoPorPeriodoViewModel model, int numeroDaPagina = 0, [FromQuery] int itensPorPagina = 20)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultadoViewModel<string>(ModelState.ObterErrosModelState()));

            var pedidos = await contexto.Pedidos
                                        .Include(i => i.Cliente)
                                        .Include(i => i.Itens)
                                        .Where(w => w.DataPedido >= model.DataInicio && w.DataPedido <= model.DataFim)
                                        .Skip(numeroDaPagina * itensPorPagina)
                                        .Take(itensPorPagina)
                                        .ToListAsync();

            if (pedidos == null)
                return BadRequest(new ResultadoViewModel<string>("Não existem pedidos entre o período especificado."));

            return Ok(new ResultadoViewModel<IEnumerable<Pedido>>(pedidos));
        }
        catch (Exception ex)
        {
            LogUtil.LogExceptionController(ex, "PedidoController", "ObterPedidoPorPeriodo");
            return StatusCode(500, new ResultadoViewModel<string>("Falha interna no servidor"));
        }
    }
}