using ApiPedidoVenda.Data;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.Repositorio.Base;

namespace ApiPedidoVenda.Repositorio;
public class RepositorioProduto : IRepositorio<Produto>
{
    private readonly ContextoPedidoVenda _contexto;

    public RepositorioProduto(ContextoPedidoVenda contexto)
    {
        _contexto = contexto;
    }

    public async void AtualizarAsync(Produto entidade)
    {
        try
        {
            _contexto.Produtos.Update(entidade);
            await _contexto.SaveChangesAsync();
        }
        catch { throw; }
    }

    public async void DeletarAsync(Produto entidade)
    {
        try
        {
            _contexto.Produtos.Remove(entidade);
            await _contexto.SaveChangesAsync();
        }
        catch { throw; }
    }

    public async void IncluirAsync(Produto entidade)
    {
        try
        {
            await _contexto.Produtos.AddAsync(entidade);
            await _contexto.SaveChangesAsync();
        }
        catch { throw; }
    }

    public Produto? ObterPorIdAsync(int id)
    {
        try
        {
            return  _contexto.Produtos.FirstOrDefault(f => f.Id == id);
        }
        catch { throw; }
    }

    public IEnumerable<Produto> ObterTodosAsync(int numeroDaPagina, int itensPorPagina)
    {
        try
        {
            return _contexto.Produtos
                            .Skip(numeroDaPagina * itensPorPagina)
                            .Take(itensPorPagina)
                            .ToList();
        }
        catch { throw; }
    }
}