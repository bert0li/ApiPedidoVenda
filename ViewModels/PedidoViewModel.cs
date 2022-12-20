using ApiPedidoVenda.Models;

namespace ApiPedidoVenda.ViewModels
{
    public class PedidoViewModel
    {
        public ClienteViewModel? Cliente { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }
        public List<ProdutoViewModel> Itens { get; set; } = new();
    }
}