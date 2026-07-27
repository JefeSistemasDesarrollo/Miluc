//using System;
//using System.Collections.Generic;

//namespace Miluc.Shared.Models.Response
//{
//    public class PaginacionNominaResponse<T>
//    {
//        // La lista de registros (ej. Empleados, Contratos, etc.)
//        public List<T> Data { get; set; } = new();

//        // Metadatos que tu componente de Blazor va a leer para armar los botones
//        public int PaginaActual { get; set; }
//        public int TotalPaginas { get; set; }
//        public int CantidadPorPagina { get; set; }
//        public int TotalRegistros { get; set; }

//        // Control de estado de la API
//        public bool Succeeded { get; set; }
//        public string? Message { get; set; }

//        // Constructor para armar la respuesta en el Servidor
//        public PaginacionNominaResponse(List<T> data, int totalRegistros, int paginaActual, int cantidadPorPagina)
//        {
//            Data = data;
//            TotalRegistros = totalRegistros;
//            CantidadPorPagina = cantidadPorPagina;
//            PaginaActual = paginaActual;

//            // Cálculo del total de páginas usando el techo decimal
//            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)cantidadPorPagina);
//            Succeeded = true;
//        }

//        // Constructor vacío necesario para que Blazor pueda deserializar el JSON
//        public PaginacionNominaResponse() { }
//    }
//}