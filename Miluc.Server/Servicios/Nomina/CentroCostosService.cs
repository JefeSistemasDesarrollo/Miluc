using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.CargosDto;
using Miluc.Shared.DTOs.Nomina.CentroDeCostos;

namespace Miluc.Server.Servicios.Nomina
{
    public class CentroCostosService(NominaDbContext _context) : ICentroCostosService

    {


        public async Task<(List<CentroCostosReaderDto> Data, int TotalRegistros)> GetCentroCostosAsync(string? filtro = null, int page = 1, int? cantidad = null, string? correo = null)

        {
            try
            {
                int cantidadTop = cantidad ?? 20;
                filtro ??= string.Empty;

                var query = _context.CentroCostos.AsNoTracking().AsQueryable();


                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string filtroMinuscula = filtro.Trim().ToLower();
                    query = query.Where(c => c.Nombre.ToLower().Contains(filtroMinuscula));
                }


                int totalRegistros = await query.CountAsync();


                var centrosCostos = await query
                    .Select(c => new CentroCostosReaderDto
                    {
                        CentroCostoId = c.CentroCostoId,
                        Nombre = c.Nombre,
                        Codigo = c.Codigo,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Activo = c.Activo
                    }).OrderBy(c => c.Nombre)
                    .Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .ToListAsync();

                return (centrosCostos, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener centros de costos: {ex.Message}", ex);
            }
        }

        public Task<CentroCostosReaderDto> GetCentroCostosByIdAsync(int id)
        {
            try
            {
                var centroCostos = _context.CentroCostos.AsNoTracking()
                    .Where(c => c.CentroCostoId == id)
                    .Select(c => new CentroCostosReaderDto
                    {
                        CentroCostoId = c.CentroCostoId,
                        Nombre = c.Nombre,
                        Codigo = c.Codigo,

                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Activo = c.Activo
                    }).FirstOrDefaultAsync();
                if (centroCostos == null)
                {
                    throw new Exception($"Centro de costos con ID {id} no encontrado.");
                }
                return centroCostos;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener centro de costos por ID: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpsertCentoCostosAsync(UpsertCentroCostosDto centroCostos)
        {
            try
            {
                var nombre = centroCostos.Nombre?.Trim().ToUpper();
                var codigo = centroCostos.Codigo?.Trim().ToUpper();

                // Validar código duplicado
                bool codigoExiste = await _context.CentroCostos
                    .AnyAsync(c =>
                        c.Codigo == codigo &&
                        c.CentroCostoId != centroCostos.CentroCostoId);

                if (codigoExiste)
                    throw new ArgumentException("Ya existe un centro de costos con ese código.");

                // Validar nombre duplicado
                bool nombreExiste = await _context.CentroCostos
                    .AnyAsync(c =>
                        c.Nombre == nombre &&
                        c.CentroCostoId != centroCostos.CentroCostoId);

                if (nombreExiste)
                    throw new ArgumentException("Ya existe un centro de costos con ese nombre.");

                var centroCostosExistente = await _context.CentroCostos
                    .FirstOrDefaultAsync(c => c.CentroCostoId == centroCostos.CentroCostoId);

                if (centroCostosExistente == null)
                {
                    var nuevoCentro = new CentroCostos
                    {
                        Nombre = nombre,
                        Codigo = codigo,
                        FechaCreacion = DateTime.UtcNow,
                        FechaActualizacion = DateTime.UtcNow,
                        Activo = centroCostos.Activo
                    };

                    await _context.CentroCostos.AddAsync(nuevoCentro);
                }
                else
                {
                    centroCostosExistente.Nombre = nombre;
                    centroCostosExistente.Codigo = codigo;
                    centroCostosExistente.Activo = centroCostos.Activo;
                    centroCostosExistente.FechaActualizacion = DateTime.UtcNow;
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Error al guardar el centro de costos: {errorReal}");
            }
        }

        public async Task<bool> DeleteCentroCostosAsync(int centroCostoId)
        {
            try
            {
                var centroCosto = await _context.CentroCostos
                    .FindAsync(centroCostoId);

                if (centroCosto == null)
                {
                    throw new Exception(
                        $"No se encontró el centro de costos con ID {centroCostoId}");
                }

                var contratosAsociados = await _context.ContratoLaboral
                    .CountAsync(x => x.CentroCostoId == centroCostoId);

                if (contratosAsociados > 0)
                {
                    throw new Exception(
                        $"No se puede eliminar el centro de costos porque está siendo utilizado por {contratosAsociados} contrato(s).");
                }

                _context.CentroCostos.Remove(centroCosto);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
