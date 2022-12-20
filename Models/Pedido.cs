namespace ApiPedidoVenda.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }
        public bool Cancelado { get; set; }
        public List<Produto> Itens { get; set; } = new();

        public void AddItem(Produto item)
        {
            Itens.Add(item);
            ValorTotal += item.Valor;
        }

        public void RemoverItem(Produto item)
        {
            if (Itens.Contains(item))
            {
                Itens.Remove(item);
                ValorTotal -= item.Valor;
            }
        }
    }
}