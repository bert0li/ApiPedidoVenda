using ApiPedidoVenda.Data;
using ApiPedidoVenda.Extensions;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidoVenda.Controllers
{
    [ApiController]
    public class PedidoController : ControllerBase
    {
        [HttpGet("v1/pedidos")]
        public async Task<IActionResult> ObterTodosAsync([FromServices] ContextoPedidoVenda contexto)
        {
            try
            {
                var pedidos = await contexto.Pedidos
                                            .Include(i => i.Cliente)
                                            .Include(i => i.Itens)
                                            .ToListAsync();

                return Ok(new ResultadoViewModel<IEnumerable<Pedido>>(pedidos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultadoViewModel<Pedido>("Falha interna no servidor."));
            }
        }

        [HttpGet("v1/pedidos/{id:int}")]
        public async Task<IActionResult> ObterPorIdAsync([FromServices] ContextoPedidoVenda contexto,[FromRoute] int id)
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
                return StatusCode(500, new ResultadoViewModel<Pedido>("Falha interna no servidor"));
            }
        }

        [HttpPost("v1/pedidos")]
        public async Task<IActionResult> IncluirAsync([FromServices] ContextoPedidoVenda contexto, [FromBody] PedidoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultadoViewModel<Pedido>(ModelState.ObterErrosModelState()));

                if (model.Itens == null || model.Itens.Count() == 0)
                    return BadRequest(new ResultadoViewModel<Pedido>("Pedido não tem itens."));

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
                return StatusCode(500, new ResultadoViewModel<Pedido>("Falha interna no servidor"));
            }
        }
    }
}