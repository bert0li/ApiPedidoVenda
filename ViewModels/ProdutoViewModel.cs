using System.ComponentModel.DataAnnotations;

namespace ApiPedidoVenda.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(1, 9999999999999999.99, ErrorMessage = "Valor deve ser maior do que 0 e menor que 9999999999999999.99.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Informe se o produto esta ativo ou inativo")]
        public bool Ativo { get; set; }
    }
}