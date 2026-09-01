using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using System.Linq.Expressions;

namespace Miluc.Server.Servicios.Nomina
{
    public class TipoContratoService(NominaDbContext _contex) : ITipoContratoService
    {


        public async Task<(List<TipoContratoReaderDto> Data, int TotalRegistros)> GetTipoContratoAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            {
                try
                {
                    int cantidadtop = cantidad ?? 20;
                    var query = _contex.TipoContrato.AsNoTracking().AsQueryable();

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        query = query.Where(e => e.NombreContrato.Contains(filtro));
                    }
                    var totalRegistros = query.Count();

                    var epsList = await query
                        .Select(e => new TipoContratoReaderDto
                        {
                            TipoContratoId = e.TipoContratoId,
                            NombreContrato = e.NombreContrato
                        })
                        .Skip((page - 1) * cantidadtop)
                        .Take(cantidadtop)
                        .ToListAsync();
                    return (epsList, totalRegistros);


                }

                catch (Exception ex)
                {

                    throw new Exception($"Error al obtener las Eps: {ex.Message}");

                }


            }
        }
    }
}

