//using System;

//namespace Miluc.Shared.DTOs.Nomina.PaginacionNomina
//{
//    public class PaginacionNominaRequest
//    {
//        private const int MaxPageSize = 100;
//        private int _cantidad = 10;
//        private string? _filtro;

//        // propiedad arranca en 1 para evitar el error de paginacion
//        public int Pagina { get; set; } = 1;

//        public int Cantidad
//        {
//            get => _cantidad;
//            set => _cantidad = (value > MaxPageSize) ? MaxPageSize : value;
//        }

//        public string? Filtro
//        {
//            get => _filtro;
//            set => _filtro = string.IsNullOrEmpty(value) ? null : value.Trim();
//        }
//    }
//}
