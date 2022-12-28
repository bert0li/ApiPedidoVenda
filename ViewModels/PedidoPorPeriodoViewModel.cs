using System.ComponentModel.DataAnnotations;

namespace ApiPedidoVenda.ViewModels
{
    public class PedidoPorPeriodoViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }
    }
}
