namespace ApiPedidoVenda.Util
{
    public sealed class LogUtil
    {
        public static void LogExceptionController(Exception exception, string controller, string action)
            => Console.WriteLine($"[Controller]: {controller}\n[Action]:{action}\n[Mensagem Exception]: {exception.Message}\n[Exception StackTrace]: {exception.StackTrace}");            
    }
}