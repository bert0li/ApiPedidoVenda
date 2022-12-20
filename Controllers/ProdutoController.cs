using ApiPedidoVenda.Data;
using ApiPedidoVenda.Extensions;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidoVenda.Controllers
{
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        [HttpGet("v1/produtos")]
        public async Task<IActionResult> ObterTodosAsync([FromServices] ContextoPedidoVenda contexto)
        {
            try
            {
                var produtos = await contexto.Produtos.ToListAsync();

                return Ok(new ResultadoViewModel<IEnumerable<Produto>>(produtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultadoViewModel<Produto>("Falha interna no servidor"));
            }
        }

        [HttpGet("v1/produtos/{id:int}")]
        public async Task<IActionResult> ObterPorIdAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id)
        {
            try
            {
                var produto = await contexto.Produtos.FirstOrDefaultAsync(f => f.Id == id);

                if (produto == null)
                    return BadRequest(new ResultadoViewModel<Produto>($"Produto com código [{id}] não encontrado."));

                return Ok(new ResultadoViewModel<Produto>(produto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultadoViewModel<Produto>("Falha interna no servidor"));
            }
        }

        [HttpPost("v1/produtos")]
        public async Task<IActionResult> IncluirAsync([FromServices] ContextoPedidoVenda contexto, [FromBody] ProdutoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultadoViewModel<Produto>(ModelState.ObterErrosModelState()));

                var produto = new Produto()
                {
                    Id = 0,
                    Nome = model.Nome,
                    Valor = model.Valor,
                    Ativo = model.Ativo
                };

                await contexto.Produtos.AddRangeAsync(produto);
                await contexto.SaveChangesAsync();

                return Created($"v1/produtos/{produto.Id}", new ResultadoViewModel<Produto>(produto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultadoViewModel<Produto>("Falha interna no servidor."));
            }
        }

        [HttpPut("v1/produtos/{id:int}")]
        public async Task<IActionResult> AtualizarAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id, [FromBody] ProdutoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultadoViewModel<Produto>(ModelState.ObterErrosModelState()));

                var produto = await contexto.Produtos.FirstOrDefaultAsync(f => f.Id == id);

                if (produto == null)
                    return BadRequest(new ResultadoViewModel<Produto>($"Produto com código: [{id}] não encontrado."));

                produto.Nome = model.Nome;
                produto.Valor = model.Valor;
                produto.Ativo = model.Ativo;

                contexto.Produtos.Update(produto);
                await contexto.SaveChangesAsync();

                return Ok(new ResultadoViewModel<Produto>(produto));
            }
            catch (DbUpdateException up)
            {
                return StatusCode(500, new ResultadoViewModel<Produto>("Não foi possível alterar o produto."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultadoViewModel<Produto>("Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/produtos/{id:int}")]
        public async Task<IActionResult> DeletarAsync([FromServices] ContextoPedidoVenda contexto, [FromRoute] int id)
        {
            try
            {
                var produto = await contexto.Produtos.FirstOrDefaultAsync(f => f.Id == id);

                if (produto == null)
                    return BadRequest(new ResultadoViewModel<Produto>($"Produto com código: [{id}] não encontrado."));

                contexto.Produtos.Remove(produto);
                await contexto.SaveChangesAsync();

                return Ok(new ResultadoViewModel<Produto>(produto));
            }
            catch (DbUpdateException up)
            {
                return StatusCode(500, new ResultadoViewModel<Produto>("Não foi possível excluir o produto."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultadoViewModel<Produto>("Falha interna no servidor."));
            }
        }
    }
}