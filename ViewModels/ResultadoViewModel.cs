using ApiPedidoVenda.Models;

namespace ApiPedidoVenda.ViewModels
{
    public class ResultadoViewModel<T> where T : class
    {
        private Task<IEnumerable<Cliente>> clientes;

        public T Entidade { get; set; } = null!;
        public List<string> Erros { get; private set; } = new();

        public ResultadoViewModel(T entidade, List<string> erros)
        {
            Entidade = entidade;
            Erros = erros;
        }

        public ResultadoViewModel(T entidade)
        {
            Entidade = entidade;
        }

        public ResultadoViewModel(List<string> erros)
        {
            Erros = erros;
        }

        public ResultadoViewModel(string erros)
        {
            Erros.Add(erros);
        }

        public ResultadoViewModel(Task<IEnumerable<Cliente>> clientes)
        {
            this.clientes = clientes;
        }
    }
}