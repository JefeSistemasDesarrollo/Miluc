using System.Runtime.CompilerServices;

namespace Miluc.Shared.Models.Response
{
    public class PaginacionRequest
    {
       private const int MaxPageSize = 100;
        public int Pagina { get; set; }
        private int _cantidad = 10;

        public int Cantidad
        {
            get => _cantidad;
            set => _cantidad = value > MaxPageSize ? MaxPageSize : value;

        }
        public string? Filtro { get; set; }

    }
}
