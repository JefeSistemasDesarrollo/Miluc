namespace Miluc.Shared.Models.Response
{
    public class ResponseAPI<T>
    {
        public bool EsCorrecto { get; set; }
        public T? Valor { get; set; }
        public string? Mensaje { get; set; }
        public List<string>? Errores { get; set; }
        public int CantRegistros { get; set; }

        public ResponseAPI<T> SuccessResponse(bool IsSuccess, string message, T? Value, int cantidad)
        {
            return new ResponseAPI<T>
            {
                EsCorrecto = IsSuccess,
                Mensaje = message,
                Valor = Value,
                CantRegistros = cantidad,
            };
        }
        public ResponseAPI<T> ErroresResponse(bool IsSuccessbool, string message, List<string> Errores)
        {
            return new ResponseAPI<T>
            {
                EsCorrecto = IsSuccessbool,
                Mensaje = message,
                Errores = Errores,


            };

        }
    }
}
