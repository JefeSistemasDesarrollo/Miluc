using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Shared.Models.Response;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;

namespace Miluc.Server.Servicios.Nomina
{
    public class EpsService(NominaDbContext _context) : IEpsService
    {
        public async Task<EpsReaderDto> CreateEpsAsync(EpsCreateDto EpsCreateDto)
        {
            if (EpsCreateDto == null)
                throw new ArgumentException("La información de la EPS no puede ser nula.", nameof(EpsCreateDto));

            try
            {
                // 1. Validar si el Nombre ya existe
                var existeNombre = await _context.Eps.AnyAsync
                    (e => e.Nombre.ToLower().Trim() == EpsCreateDto.Nombre.ToLower().Trim());

                if (existeNombre)
                    throw new Exception("Ya existe una EPS registrada con el mismo nombre.");

                // 2. Validar si el Código ya existe (solo si enviaron un código)
                if (!string.IsNullOrWhiteSpace(EpsCreateDto.Codigo))
                {
                    var existeCodigo = await _context.Eps.AnyAsync
                        (e => e.Codigo.ToLower().Trim() == EpsCreateDto.Codigo.ToLower().Trim());

                    if (existeCodigo)
                        throw new Exception("Ya existe una EPS registrada con el mismo código.");
                }

                // 3. Crear la nueva entidad
                var nuevaEps = new Eps
                {
                    Nombre = EpsCreateDto.Nombre.Trim(),
                    Codigo = EpsCreateDto.Codigo?.Trim(),
                    FechaCreacion = DateTime.Now,
                    Activo = EpsCreateDto.Activo ,
                };

                _context.Eps.Add(nuevaEps);
                await _context.SaveChangesAsync();

                return new EpsReaderDto
                {
                    EpsId = nuevaEps.EpsId,
                    Nombre = nuevaEps.Nombre,
                    Codigo = nuevaEps.Codigo,
                    FechaCreacion = nuevaEps.FechaCreacion,
                    FechaActualizacion = null,
                    Activo = nuevaEps.Activo
                };
            }
            catch (Exception ex)
            {


                throw new Exception(ex.Message);
            }
        }

       

        public async Task<EpsReaderDto> GetByEpsAsync(int id)
        {
            try
            {
                var epsId = await _context.Eps.AsNoTracking()
                    .Where(e => e.EpsId == id)
                    .Select(e => new EpsReaderDto
                    {
                        EpsId = e.EpsId,
                        Nombre = e.Nombre,
                        Codigo = e.Codigo,
                        FechaActualizacion = DateTime.Now,
                        FechaCreacion = DateTime.Now,
                        Activo = e.Activo

                    }).FirstOrDefaultAsync() ?? throw new Exception("Eps no encontrado.");
                return epsId;


            }
            catch (Exception ex) 
                {
                throw new Exception($"Eps no encontrada:{ ex.Message}" ) ;
                }   
        }
         

        public async Task<(List<EpsReaderDto> Data, int TotalRegistros)> GetEpsAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadtop = cantidad ?? 20;
                var query = _context.Eps.AsNoTracking().AsQueryable();
                
               if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(e => e.Nombre.Contains(filtro) || e.Codigo.Contains(filtro));
                }
                var totalRegistros =  query.Count();
                
                var epsList = await query
                    .Select(e => new EpsReaderDto
                    {
                        EpsId = e.EpsId,
                        Nombre = e.Nombre,
                        Codigo = e.Codigo,
                      
                        FechaCreacion = e.FechaCreacion,
                        FechaActualizacion = e.FechaActualizacion,
                        Activo = e.Activo
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

        public async Task<EpsReaderDto> UpdateEpsAsync(UpdateEpsDto updateEps)
        {
            try
            {

                var eps = await _context.Eps.FirstOrDefaultAsync(e => e.EpsId == updateEps.EpsId) ?? throw new Exception("La EPS no existe.");

                //validar si tiene afiliacionessi tiene no se puede  inactivar eps
                if (eps.Activo && !updateEps.Activo) 
                { 
                    bool tieneAfiliaciones = await _context.AfiliacionSeguridadSocial.AnyAsync(a => a.EpsId == updateEps.EpsId);

                    if (tieneAfiliaciones)
                        throw new Exception("La eps no se puede inactivar y que hay empleados afiliados.");

                    


                }

                var nombreFormateado = updateEps.Nombre?.Trim().ToUpper();
                var codigoFormateado = updateEps.Codigo?.Trim().ToUpper();

                
                var existeEps = await _context.Eps.AnyAsync(e =>
                    e.EpsId != updateEps.EpsId &&
                    (e.Nombre.ToUpper() == nombreFormateado || e.Codigo.ToUpper() == codigoFormateado)
                );

                if (existeEps)
                    throw new Exception("Ya existe otra EPS con el mismo nombre o código.");

                
                eps.Nombre = updateEps.Nombre;
                eps.Codigo = updateEps.Codigo;
                eps.FechaActualizacion = DateTime.Now;
                eps.Activo = updateEps.Activo;

               
                await _context.SaveChangesAsync();

                
                return new EpsReaderDto
                {
                    EpsId = eps.EpsId,
                    Nombre = eps.Nombre,
                    Codigo = eps.Codigo,
                    FechaActualizacion = eps.FechaActualizacion,
                    Activo = eps.Activo 
                };
            
    }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");


            }
        }
        public async Task<bool> DeleteEpsAsync(int id)
        {
            try
            {
                var eps = await _context.Eps.FirstOrDefaultAsync(e => e.EpsId == id);

                if (eps == null)
                {
                    throw new Exception("La EPS no existe.");
                }

                // Validar si tiene afiliaciones activas o históricas vinculadas
                bool tieneEmpleadosAfiliados = await _context.AfiliacionSeguridadSocial
                    .AnyAsync(af => af.EpsId == id);

                if (tieneEmpleadosAfiliados)
                {
                    throw new Exception("La EPS no se puede eliminar porque cuenta con registros o empleados vinculados en el sistema.");
                }

                // Si no tiene registros vinculados, procedemos con seguridad
                _context.Eps.Remove(eps);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
    }

