using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiPedidoVenda.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> ObterErrosModelState(this ModelStateDictionary modelState)
        {
            var erros = new List<string>();
            foreach(var valor in modelState.Values)
                erros.AddRange(valor.Errors.Select(s => s.ErrorMessage));

            return erros;
        }
    }
}