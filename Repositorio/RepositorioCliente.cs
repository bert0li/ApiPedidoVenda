using ApiPedidoVenda.Data;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.Repositorio.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidoVenda.Repositorio
{
    public class RepositorioCliente : IRepositorio<Cliente>
    {
        private readonly ContextoPedidoVenda _contexto;

        public RepositorioCliente(ContextoPedidoVenda contexto) => _contexto = contexto;

        public async void AtualizarAsync(Cliente entidade)
        {
            try
            {
                _contexto.Clientes.Update(entidade);
                await _contexto.SaveChangesAsync();
            }
            catch { throw; }
        }

        public async void DeletarAsync(Cliente entidade)
        {
            _contexto.Clientes.Remove(entidade);
            await _contexto.SaveChangesAsync();
        }

        public async void IncluirAsync(Cliente entidade)
        {
            try
            {
                await _contexto.Clientes.AddAsync(entidade);
                await _contexto.SaveChangesAsync();
            }
            catch { throw; }
        }

        public Cliente? ObterPorIdAsync(int id)
        {
            try
            {
                return _contexto.Clientes.FirstOrDefault(f => f.Id == id);
            }
            catch { throw; }
        }

        public  IEnumerable<Cliente> ObterTodosAsync(int numeroDaPagina, int itensPorPagina)
        {
            try
            {
                return  _contexto.Clientes
                                 .Skip(numeroDaPagina * itensPorPagina)
                                 .Take(itensPorPagina)
                                 .ToList();
            }
            catch { throw; }
        }
    }
}
