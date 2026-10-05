using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class EmpresaService(NominaDbContext _context) : IEmpresaService
    {



        public async Task<List<EmpresaReaderDto>> GetEmpresasAsync(string? filtro = null,int page = 1,int? cantidad = null,string? correo = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;
                filtro ??= string.Empty;

                var query = _context.Empresa.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(e =>
                        e.NombreEmpresa.Contains(filtro) ||
                        e.Nit.Contains(filtro));
                }

                var totalRegistros = await query.CountAsync();

                var empresas = await query
                    .OrderBy(e => e.EmpresaId)
                    .Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .Select(e => new EmpresaReaderDto
                    {
                        EmpresaId = e.EmpresaId,
                        Nit = e.Nit,
                        NombreEmpresa = e.NombreEmpresa,
                        FechaCreacion = e.FechaCreacion,
                        FechaActualizacion = e.FechaActualizacion,
                        Activo = e.Activo
                    })
                    .ToListAsync();

                return empresas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las empresas: {ex.Message}");
            }
        }
        public async Task<bool> UpsertEmpresaAsync(UpsertEmpresaDto empresaUpdateDto)
        {
            try
            {
                var empresaExiste = await _context.Empresa
                    .FirstOrDefaultAsync(e => e.EmpresaId == empresaUpdateDto.EmpresaId);

                var nitExiste = await _context.Empresa
                    .AnyAsync(x =>
                        x.Nit == empresaUpdateDto.Nit &&
                        x.EmpresaId != empresaUpdateDto.EmpresaId);

                if (nitExiste)
                {
                    throw new Exception("Ya existe una empresa con ese NIT.");
                }

                if (empresaExiste == null)
                {
                    var newEmpresa = new Empresa
                    {
                        Nit = empresaUpdateDto.Nit,
                        NombreEmpresa = empresaUpdateDto.NombreEmpresa,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Activo = empresaUpdateDto.Activo
                    };

                    await _context.Empresa.AddAsync(newEmpresa);
                }
                else
                {
                    empresaExiste.Nit = empresaUpdateDto.Nit;
                    empresaExiste.NombreEmpresa = empresaUpdateDto.NombreEmpresa;
                    empresaExiste.FechaActualizacion = DateTime.Now;
                    empresaExiste.Activo = empresaUpdateDto.Activo;
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar la empresa: {ex.Message}");
            }
        }
        

        public async Task<EmpresaReaderDto> GetEmpresaByIdAsync(int empresaId)
        {
            try
            {
                var empresa = await _context.Empresa.AsNoTracking()
                    .Where(e => e.EmpresaId == empresaId)
                    .Select(e => new EmpresaReaderDto
                    {
                        EmpresaId = e.EmpresaId,
                        Nit = e.Nit,
                        NombreEmpresa = e.NombreEmpresa,
                        FechaCreacion = e.FechaCreacion,
                        FechaActualizacion = e.FechaActualizacion,// DB puede ser NULL
                        Activo = e.Activo
                    }).FirstOrDefaultAsync();
                if (empresa == null)
                {
                    throw new Exception($"No se encontró la empresa con ID {empresaId}");
                }
                return empresa;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la empresa: {ex.Message}");
            }
        }
        public async Task<bool> DeleteEmpresaAsync(int id)
        {
            try
            {
                var empresa = await _context.Empresa.FindAsync(id);
                if (empresa == null)
                {
                    throw new Exception($"No se encontró la empresa con ID {id}");
                }

                _context.Empresa.Remove(empresa);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la empresa: {ex.Message}");
            }
        }
    }
    }

