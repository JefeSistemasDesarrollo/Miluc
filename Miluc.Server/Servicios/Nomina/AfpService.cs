using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.AfpDto;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Servicios.Nomina
{
    public class AfpService(NominaDbContext _context) : IAfpService
    {
        public async Task<AfpReaderDto> CreateAfpAsync(AfpCreateDto afpCreateDto)
        {

            if (afpCreateDto == null)
                throw new ArgumentException("La información de la AFP no puede ser nula.", nameof(afpCreateDto));

            try
            {

                var existeAfp = await _context.Afp.AnyAsync
                    (a => a.Nombre.ToLower().Trim() == afpCreateDto.Nombre.ToLower().Trim());

                if (existeAfp) throw new Exception("Ya existe una AFP con el mismo nombre.");

                if (existeAfp) throw new Exception("Ya existe una ARL con el mismo nombre.");

                // Validar si el código ya existe (solo si se proporciona un código)
                if (!string.IsNullOrEmpty(afpCreateDto.Codigo))
                {
                    var existeCodigo = await _context
                       .Arl.AnyAsync
                        (a => a.Codigo.ToLower().Trim() == afpCreateDto.Codigo.ToLower().Trim());
                    if (existeCodigo) throw new Exception("Ya existe una ARL con el mismo código.");
                }


                // Mapeo de DTO a Entidad:

                var nuevaAfp = new Afp
                {
                    Nombre = afpCreateDto.Nombre,
                    Codigo = afpCreateDto.Codigo,

                    FechaCreacion = DateTime.UtcNow,
                    Activo = afpCreateDto.Activo
                };


                _context.Afp.Add(nuevaAfp);
                await _context.SaveChangesAsync();


                return new AfpReaderDto
                {
                    AfpId = nuevaAfp.AfpId,
                    Nombre = nuevaAfp.Nombre,

                    FechaCreacion = nuevaAfp.FechaCreacion,
                    Activo = nuevaAfp.Activo
                };
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al crear la AFP: {ex.Message}");
            }
        }

        public async Task<(List<AfpReaderDto> Data, int TotalRegistros)> GetAfpAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {


                int CantidadTop = cantidad ?? 20;
                var query = _context.Afp.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(a => a.Nombre.Contains(filtro) || a.Codigo.Contains(filtro));
                }
                int totalRegistros = await query.CountAsync();


                var afp = await query
                .Select(a => new AfpReaderDto
                {

                    AfpId = a.AfpId,
                    Nombre = a.Nombre,
                    Codigo = a.Codigo,

                    FechaCreacion = a.FechaCreacion,
                    FechaActualizacion = a.FechaActualizacion,
                    Activo = a.Activo,
                }).Skip((page - 1) * CantidadTop)
                  .Take(CantidadTop)
                  .ToListAsync();
                return (afp, totalRegistros);


            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Afp:{ex.Message}");
            }
        }

        public async Task<AfpReaderDto> GetByAfpAsync(int id)
        {
            try
            {
                var afpId = await _context.Afp.AsNoTracking()
                    .Where(e => e.AfpId == id)
                    .Select(e => new AfpReaderDto
                    {
                        AfpId = e.AfpId,
                        Nombre = e.Nombre,
                        Codigo = e.Codigo,
                        FechaActualizacion = DateTime.Now,
                        FechaCreacion = DateTime.Now,
                        Activo = e.Activo

                    }).FirstOrDefaultAsync();

                if (afpId == null) throw new Exception("afp no encontrado.");

                return afpId;





            }
            catch (Exception ex)
            {
                throw new Exception($"Eps no encontrada:{ex.Message}");
            }
        }


        public async Task<AfpReaderDto> UpdateAfpAsync(UpdateAfp updateAfp)
        {
            try
            {

                var afp = await _context.Afp.FirstOrDefaultAsync(a => a.AfpId == updateAfp.AfpId);
                if (afp == null)
                    throw new Exception("La Afp no existe.");


                var nombreFormateado = updateAfp.Nombre?.Trim().ToUpper();
                var codigoFormateado = updateAfp.Codigo?.Trim().ToUpper();


                var existeAfp = await _context.Afp.AnyAsync(a =>
                    a.AfpId != updateAfp.AfpId &&
                    (a.Nombre.ToUpper() == nombreFormateado || a.Codigo.ToUpper() == codigoFormateado)
                );

                if (existeAfp)
                    throw new Exception("Ya existe otra Afp con el mismo nombre o código.");

                afp.AfpId = updateAfp.AfpId;
                afp.Nombre = updateAfp.Nombre;
                afp.Codigo = updateAfp.Codigo;
                afp.FechaActualizacion = DateTime.Now;
                afp.Activo = updateAfp.Activo;


                await _context.SaveChangesAsync();


                return new AfpReaderDto
                {
                    AfpId = afp.AfpId,
                    Nombre = afp.Nombre,
                    Codigo = afp.Codigo,
                    FechaActualizacion = afp.FechaActualizacion,
                    Activo = afp.Activo
                };

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar AFP: {ex.Message}");

            }
        }

    }
}