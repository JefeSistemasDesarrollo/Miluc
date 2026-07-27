using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;

namespace Miluc.Server.Servicios.Nomina
{

    public class ArlService(NominaDbContext _contex) : IArlService

    {
        public async Task<ArlReaderDto> CreateArlAsync(ArlCreateDto arlCreateDto)
        {
            if (arlCreateDto == null)
                throw new ArgumentNullException(nameof(arlCreateDto), "El objeto ArlCreateDto no puede ser nulo.");
            try
            {
                var existeArl = await _contex.Arl.AnyAsync
                    (a => a.Nombre.ToLower().Trim() == arlCreateDto.Nombre.ToLower().Trim());
                if (existeArl) throw new Exception("Ya existe una ARL con el mismo nombre.");

                // Validar si el código ya existe (solo si se proporciona un código)
                if (!string.IsNullOrEmpty(arlCreateDto.Codigo))
                {
                    var existeCodigo = await _contex.Arl.AnyAsync
                        (a => a.Codigo.ToLower().Trim() == arlCreateDto.Codigo.ToLower().Trim());
                    if (existeCodigo) throw new Exception("Ya existe una ARL con el mismo código.");
                }



                var arl = new Arl
                {
                    Nombre = arlCreateDto.Nombre,
                    Codigo = arlCreateDto.Codigo,

                    FechaCreacion = DateTime.UtcNow,
                    Activo = arlCreateDto.Activo,
                };
                _contex.Arl.Add(arl);
                await _contex.SaveChangesAsync();
                return new ArlReaderDto
                {
                    ArlId = arl.ArlId,
                    Nombre = arl.Nombre,
                    Codigo = arl.Codigo,

                    FechaCreacion = arl.FechaCreacion,
                    Activo = arl.Activo
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear nueva ARL: {ex.Message}");
            }
        }

        public async Task<(List<ArlReaderDto> Data, int TotalRegistros)> GetArlAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            {
                try
                {
                    int cantidadtop = cantidad ?? 20;
                    var query = _contex.Arl.AsNoTracking().AsQueryable();


                    if (!string.IsNullOrWhiteSpace(filtro))
                    {
                        query = query.Where(a => a.Nombre.Contains(filtro) || a.Codigo.Contains(filtro));
                    }


                    int totalRegistros = await query.CountAsync();


                    var cajaComp = await query
                        .Select(c => new ArlReaderDto
                        {
                            ArlId = c.ArlId,
                            Nombre = c.Nombre,
                            Codigo = c.Codigo,

                            FechaCreacion = c.FechaCreacion,
                            FechaActualizacion = c.FechaActualizacion,
                            Activo = c.Activo,
                        })
                        .Skip((page - 1) * cantidadtop)
                        .Take(cantidadtop)
                        .ToListAsync();


                    return (cajaComp, totalRegistros);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Al obtener Caja Compensacion: {ex.Message}");
                }
            }





        }
    }
}
