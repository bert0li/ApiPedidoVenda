namespace ApiPedidoVenda.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public bool Ativo { get; set; }
        public List<Pedido> Pedidos { get; set; }
    }
}