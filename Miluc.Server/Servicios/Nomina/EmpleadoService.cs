using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.Models.Response;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class EmpleadoService(NominaDbContext _context) : IEmpleadoService
    {
        public async Task<EmpleadoReaderDto> CreateEmpleadosAsync(EmpleadoCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {   // validacion Trim() para evitar espacios en blanco y posibles duplicados visuales.
                string documentoLimpio = dto.Documento != null ? dto.Documento.Trim() : string.Empty;

                // 1. Validar duplicado AFUERA del try-catch
                var existe = await _context.Empleado.AnyAsync(e => e.Documento == documentoLimpio);
                if (existe)
                {
                    throw new Exception("El número de documento ya se encuentra registrado.");
                }

                try
                {
                    var nuevoEmpleado = new Empleado
                    {
                        CodigoMunicipio = dto.CodigoMunicipio,
                        TipoDocumentoId = dto.TipoDocumentoId ?? 0,
                        Documento = documentoLimpio,
                        FechaExpedicionDoc = dto.FechaExpedicionDoc.Value,

                        PrimerNombre = dto.PrimerNombre.Trim(),
                        SegundoNombre = dto.SegundoNombre?.Trim(),
                        PrimerApellido = dto.PrimerApellido.Trim(),
                        SegundoApellido = dto.SegundoApellido?.Trim(),
                        FechaNacimiento = dto.FechaNacimiento.Value,
                        EstadoCivilId = dto.EstadoCivilId ?? 0,
                        Celular = dto.Celular,
                        CelularAlterno = dto.CelularAlterno,
                        CorreoElectronico = dto.CorreoElectronico.Trim(),

                        Direccion = dto.Direccion.Trim(),
                        Barrio = dto.Barrio.Trim(),
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Activo = true
                    };

                    await _context.Empleado.AddAsync(nuevoEmpleado);
                    await _context.SaveChangesAsync();

                    return new EmpleadoReaderDto
                    {
                        EmpleadoId = nuevoEmpleado.EmpleadoId,
                        Documento = nuevoEmpleado.Documento,
                        PrimerNombre = nuevoEmpleado.PrimerNombre,
                        PrimerApellido = nuevoEmpleado.PrimerApellido,
                        Activo = nuevoEmpleado.Activo,
                        FechaCreacion = nuevoEmpleado.FechaCreacion
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception( ex.Message);
                }
            }
            finally
            {

            }
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            try
            {
                var delete = await _context.Empleado
                    .Where(x => x.EmpleadoId == id)
                    .FirstOrDefaultAsync();

                if (delete == null)
                {
                    return false;
                }
                else _context.Empleado.Remove(delete);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)

            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<EmpleadoReaderDto> GetEmpleadoByIdAsync(int id)
        {
            try
            {
                var empleado = await _context.Empleado.AsNoTracking()
                    .Include(e => e.Municipio)
                    .Include(e => e.TipoDocumento)
                    .Include(e => e.EstadoCivil)
                    .Where(e => e.EmpleadoId == id)
                    .Select(e => new EmpleadoReaderDto
                    {
                        EmpleadoId = e.EmpleadoId,
                        CodigoMunicipio = e.Municipio.Codigo,
                        NombreMunicipio = e.Municipio.Nombre,
                        TipoDocumentoId = e.TipoDocumento.TipoDocumentoId,
                        Documento = e.Documento,
                        FechaExpedicionDoc = e.FechaExpedicionDoc,
                        PrimerNombre = e.PrimerNombre,
                        SegundoNombre = e.SegundoNombre,
                        PrimerApellido = e.PrimerApellido,
                        SegundoApellido = e.SegundoApellido,
                        FechaNacimiento = e.FechaNacimiento,
                        EstadoCivilId = e.EstadoCivil.EstadoCivilId,
                        Celular = e.Celular,
                        CelularAlterno = e.CelularAlterno,
                        CorreoElectronico = e.CorreoElectronico,
                        Direccion = e.Direccion,
                        Barrio = e.Barrio,
                        FechaCreacion = e.FechaCreacion,
                        FechaActualizacion = e.FechaActualizacion,
                        Activo = e.Activo
                    })

                    .FirstOrDefaultAsync();

                if (empleado == null) throw new Exception("Empleado no encontrado.");

                return empleado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el empleado: {ex.Message}");
            }
        }

      

        public async Task<ResponseAPI<List<EmpleadoReaderDto>>> GetEmpleadosAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                if (filtro == null){
                    filtro = string.Empty;
                }
          
                var query = _context.Empleado.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(e => e.Documento.Contains(filtro.Trim()) ||
                                             e.PrimerNombre.Contains(filtro.Trim()) ||
                                             e.PrimerApellido.Contains(filtro.Trim()));
                }

                var totalRegistros = query.Count();

                var empleados = query
                   .Include(e => e.Municipio)
                   .Select(e => new EmpleadoReaderDto
                   {
                       EmpleadoId = e.EmpleadoId,
                       CodigoMunicipio = e.CodigoMunicipio ?? string.Empty,
                       // EF Core hace el JOIN implícito aquí, no necesitas el .Include() arriba
                       NombreMunicipio = e.Municipio != null ? e.Municipio.Nombre : string.Empty,
                       TipoDocumentoId = e.TipoDocumentoId,
                       Documento = e.Documento ?? string.Empty,
                       FechaExpedicionDoc = e.FechaExpedicionDoc,

                       PrimerNombre = e.PrimerNombre ?? string.Empty,
                       SegundoNombre = e.SegundoNombre ?? string.Empty,
                       PrimerApellido = e.PrimerApellido ?? string.Empty,
                       SegundoApellido = e.SegundoApellido ?? string.Empty,

                       FechaNacimiento = e.FechaNacimiento,
                       EstadoCivilId = e.EstadoCivilId,

                       Celular = e.Celular ?? string.Empty,
                       CelularAlterno = e.CelularAlterno ?? string.Empty,
                       CorreoElectronico = e.CorreoElectronico ?? string.Empty,
                       Direccion = e.Direccion ?? string.Empty,
                       Barrio = e.Barrio ?? string.Empty,

                       FechaCreacion = e.FechaCreacion,
                       FechaActualizacion = e.FechaActualizacion,
                       Activo = e.Activo
                   
                   })
                   .OrderBy(e => e.EmpleadoId) //  Solución al warning de ordenamiento
                   .Skip((page - 1) * cantidadTop)
                   .Take(cantidadTop)
                   .ToList();

                if (empleados == null || empleados.Count == 0)
                {
                    return new ResponseAPI<List<EmpleadoReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron empleados",
                        CantRegistros = 0,
                    };
                }

                return new ResponseAPI<List<EmpleadoReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = empleados,
                    CantRegistros = totalRegistros,
                    Mensaje = "Empleados obtenidos correctamente."
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los empleados: {ex.Message}");
            }
        }

        public async Task<EmpleadoReaderDto> UpdateEmpleadosAsync(EmpleadoUpdateDto dto)
        {
            try
            {
                
                // Busca el empleado en la base de datos por su ID de forma asíncrona.
                var empleado = await _context.Empleado
                    .FirstOrDefaultAsync(e => e.EmpleadoId == dto.EmpleadoId);

                // Si no se encuentra el registro, se interrumpe el flujo con una excepción.
                if (empleado == null)
                    throw new Exception("Empleado no existe");

               
                var existeDocumento = await _context.Empleado
                    .AnyAsync(e => e.Documento.ToLower() == dto.Documento.ToLower()
                                   && e.EmpleadoId != dto.EmpleadoId); 
                if (existeDocumento)
                    throw new Exception("Ya existe un empleado con ese documento");

               
                empleado.CodigoMunicipio = dto.CodigoMunicipio;
                empleado.TipoDocumentoId = dto.TipoDocumentoId;
                empleado.Documento = dto.Documento;
                empleado.FechaExpedicionDoc = dto.FechaExpedicionDoc.Value;

               
                empleado.PrimerNombre = dto.PrimerNombre.ToUpper();
                empleado.SegundoNombre = dto.SegundoNombre?.ToUpper();
                empleado.PrimerApellido = dto.PrimerApellido.ToUpper();
                empleado.SegundoApellido = dto.SegundoApellido?.ToUpper();

                empleado.FechaNacimiento = dto.FechaNacimiento.Value;
                empleado.EstadoCivilId = dto.EstadoCivilId.Value;

                empleado.Celular = dto.Celular;
                empleado.CelularAlterno = dto.CelularAlterno;

                
                empleado.CorreoElectronico = dto.CorreoElectronico.ToLower();
                empleado.Direccion = dto.Direccion;
                empleado.Barrio = dto.Barrio;

                
                empleado.FechaActualizacion = DateTime.Now;
                empleado.Activo = dto.Activo;

                
                await _context.SaveChangesAsync();

              
                return new EmpleadoReaderDto
                {
                    EmpleadoId = empleado.EmpleadoId,
                    CodigoMunicipio = empleado.CodigoMunicipio,
                    TipoDocumentoId = empleado.TipoDocumentoId,
                    Documento = empleado.Documento,
                    FechaExpedicionDoc = empleado.FechaExpedicionDoc,
                    PrimerNombre = empleado.PrimerNombre,
                    SegundoNombre = empleado.SegundoNombre,
                    PrimerApellido = empleado.PrimerApellido,
                    SegundoApellido = empleado.SegundoApellido,
                    FechaNacimiento = empleado.FechaNacimiento,
                    EstadoCivilId = empleado.EstadoCivilId,
                    Celular = empleado.Celular,
                    CelularAlterno = empleado.CelularAlterno,
                    CorreoElectronico = empleado.CorreoElectronico,
                    Direccion = empleado.Direccion,
                    Barrio = empleado.Barrio,
                    FechaCreacion = empleado.FechaCreacion, // Se preserva la fecha original de creación.
                    FechaActualizacion = empleado.FechaActualizacion, // Se expone la nueva fecha de modificación.
                    Activo = empleado.Activo
                };
            }
            catch (Exception ex)
            {
               
                // Captura cualquier fallo (de conexión, de base de datos, etc.) y lo encapsula en un mensaje claro.
                throw new Exception($"Error al actualizar el empleado: {ex.Message}");
            }
        }
    }
}