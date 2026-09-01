using Microsoft.EntityFrameworkCore;
using Miluc.Client.Interfaces.Nomina;
using Miluc.Server.Data;
using Miluc.Shared.DTOs.Nomina.Cargos;

namespace Miluc.Server.Servicios.Nomina
{
    public class CargoService(NominaDbContext _context) : ICargoService
    {
        public async Task<(List<CargosDto> Data, int TotalRegistros)> GetCargosAsyc(string? filtro = null, int page = 1,int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                var query = _context.Cargos
                    .AsNoTracking()
                    .AsQueryable();


                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string filtroMinuscula = filtro.Trim().ToLower();
                    query = query.Where(c => c.CargoNombre.ToLower().Contains(filtroMinuscula));
                }


                int totalRegistros = await query.CountAsync();

            
                var cargos = await query
                    .Select(c => new CargosDto
                    {
                        CargoId = c.CargoId,
                        CargoNombre = c.CargoNombre,
                        Activo = c.Activo
                    })
                    .Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .ToListAsync();

                return (cargos, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener cargos: {ex.Message}", ex);
            }
        }
    }
}