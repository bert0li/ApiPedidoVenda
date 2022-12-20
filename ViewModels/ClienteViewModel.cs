using System.ComponentModel.DataAnnotations;

namespace ApiPedidoVenda.ViewModels
{
    public class ClienteViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail invalido.")]
        public string Email { get; set; }
    }
}