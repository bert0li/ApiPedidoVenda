using ApiPedidoVenda.Data;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.Repositorio.Base;

namespace ApiPedidoVenda.Repositorio
{
    public class RepositorioPedido : IRepositorio<Pedido>
    {
        private readonly ContextoPedidoVenda _contexto;

        public RepositorioPedido(ContextoPedidoVenda contexto)
        {
            _contexto = contexto;
        }

        public async void AtualizarAsync(Pedido entidade)
        {
            try
            {
                _contexto.Pedidos.Update(entidade);
                await _contexto.SaveChangesAsync();
            }
            catch { throw; }
        }

        public async void DeletarAsync(Pedido entidade)
        {
            try
            {
                _contexto.Pedidos.Remove(entidade);
                await _contexto.SaveChangesAsync();
            }
            catch { throw; }
        }

        public async void IncluirAsync(Pedido entidade)
        {
            try
            {
                await _contexto.Pedidos.AddAsync(entidade);
                await _contexto.SaveChangesAsync();
            }
            catch { throw; }
        }

        public Pedido? ObterPorIdAsync(int id)
        {
            try
            {
                return _contexto.Pedidos.FirstOrDefault(f => f.Id == id);
            }
            catch { throw; }
        }

        public IEnumerable<Pedido> ObterTodosAsync(int numeroDaPagina, int itensPorPagina)
        {
            try
            {
                return _contexto.Pedidos
                                .Skip(numeroDaPagina * itensPorPagina)
                                .Take(itensPorPagina)
                                .ToList();
            }
            catch { throw; }
        }
    }
}