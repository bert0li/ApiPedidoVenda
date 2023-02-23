namespace ApiPedidoVenda.Repositorio.Base;

public interface IRepositorio<T> where T : class
{
    IEnumerable<T> ObterTodosAsync(int numeroDaPagina, int itensPorPagina);

    T? ObterPorIdAsync(int id);

    void IncluirAsync(T entidade);

    void AtualizarAsync(T entidade);

    void DeletarAsync(T entidade);
}
