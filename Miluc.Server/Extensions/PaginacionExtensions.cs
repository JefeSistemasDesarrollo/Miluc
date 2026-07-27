//using Microsoft.EntityFrameworkCore;
//using Miluc.Shared.DTOs.Nomina.PaginacionNomina;
//using Miluc.Shared.Models.Response; // Aquí están tus dos DTOs seguros
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Miluc.Server.Extensions
//{
//    public static class PaginacionExtensions
//    {
//        /// <summary>
//        /// Extensión genérica para IQueryable que aplica paginación eficiente en SQL Server.
//        /// </summary>
//        public static async Task<PaginacionNominaResponse<T>> PaginarNominaAsync<T>(
//            this IQueryable<T> query,
//            PaginacionNominaRequest request)
//        {
//            // 1. Contamos el total de registros en la BD con los filtros actuales
//            int totalRegistros = await query.CountAsync();

//            // 2. Extraemos quirúrgicamente solo las filas de la página solicitada
//            var datos = await query
//                .Skip((request.Pagina - 1) * request.Cantidad)
//                .Take(request.Cantidad)
//                .ToListAsync();

//            // 3. Devolvemos el paquete completo con su data y metadatos
//            return new PaginacionNominaResponse<T>(datos, totalRegistros, request.Pagina, request.Cantidad);
//        }
//    }
//}