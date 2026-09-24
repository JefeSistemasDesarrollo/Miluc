using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Autorizacion;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.Models.Response;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

namespace Miluc.Server.Servicios.Nomina
{
    public class EmpleadoService(NominaDbContext _context, MilucDbContext _milucDbContext, IEmailService _emailService) : IEmpleadoService
    {
        public async Task<EmpleadoReaderDto> CreateEmpleadosAsync(EmpleadoCreateDto dto)
        {
            //if (dto != null) 
            if (dto == null)
                throw new ArgumentNullException("El objeto del viene vacío", nameof(dto));
            if (await _milucDbContext.Usuarios.AnyAsync(u => u.Email.ToLower() == dto.Usuario.Email.ToLower()))
                throw new InvalidOperationException("El correo electrónico ya está en uso.");

            if (string.IsNullOrWhiteSpace(dto.Usuario.Password))
                throw new InvalidOperationException("La contraseña es obligatoria");

            using var transaction = await _context.Database.BeginTransactionAsync();

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

                    //usuario 
                    using var hmac = new System.Security.Cryptography.HMACSHA512();

                    var usuario = new Usuario
                    {
                        UserName = dto.Usuario.Email,
                        Nombres = dto.Usuario.Nombres,
                        Apellidos = dto.Usuario.Apellidos,
                        Email = dto.Usuario.Email,
                        Telefono = dto.Usuario.Telefono,
                        Foto = dto.Usuario.Foto,
                        TwoFactorEnabled = dto.Usuario.TwoFactorEnabled,
                        DebeCambiarPassword = true,
                        PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Usuario.Password)),
                        Salt = hmac.Key,
                        Activo = true,
                        HoraInicio = dto.Usuario.HoraInicio,
                        HoraFin = dto.Usuario.HoraFin,
                        CodVendedorSAP = dto.Usuario.CodVendedorSAP ?? -1,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now
                    };



                    await _milucDbContext.Usuarios.AddAsync(usuario);
                    // Guardamos para generar el IdUsuario
                    await _milucDbContext.SaveChangesAsync();

                    // 4. Asignación de Roles (Tabla Intermedia)
                    if (dto.Usuario.RolesIds != null && dto.Usuario.RolesIds.Any())
                    {
                        var userRoles = dto.Usuario.RolesIds.Select(rolId => new UsuarioRol
                        {
                            IdUsuario = usuario.IdUsuario,
                            IdRol = rolId
                        });
                        await _milucDbContext.UsuarioRoles.AddRangeAsync(userRoles);
                    }

                    // 5. Asignación de Tipos de Usuario (Tabla Intermedia)
                    if (dto.Usuario.TiposUsuarioIds != null && dto.Usuario.TiposUsuarioIds.Any())
                    {
                        var userTipos = dto.Usuario.TiposUsuarioIds.Select(tipoId => new UsuarioTipoUsuario
                        {
                            IdUsuario = usuario.IdUsuario,
                            IdTipoUsuario = tipoId
                        });
                        await _milucDbContext.UsuarioTipoUsuario.AddRangeAsync(userTipos);
                    }
                    // Guardamos las relaciones
                    await _milucDbContext.SaveChangesAsync();

                    await _emailService.SendAsync(
                           dto.Usuario.Email,
                          "Bienvenido a Miluc",
                          $"Su cuenta ha sido creada exitosamente ingrese a la platforma: <b>{"https://localhost:7198/login"}</b>. su usuarios {dto.Usuario.Email} su contraseña es  {dto.Usuario.Password} ",
                          true);


                    // 6. Confirmamos cambios
                    await transaction.CommitAsync();


                    //await _emailService.SendEmailAsync(dto.Usuario.Email, "Bienvenido a Miluc", "Su cuenta ha sido creada exitosamente .", true);




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
                    await transaction.RollbackAsync();

                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                await transaction.DisposeAsync();


            }

            throw new ArgumentNullException(nameof(dto));
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



        public async Task<ResponseAPI<List<EmpleadoReaderDto>>> GetEmpleadosAsync(string? filtro = null, int page = 1, int? cantidad = null, string? correo = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                if (filtro == null)
                {
                    filtro = string.Empty;
                }

                var query = _context.Empleado.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(e => e.Documento.Contains(filtro.Trim()) ||
                                             e.PrimerNombre.Contains(filtro.Trim()) ||
                                             e.PrimerApellido.Contains(filtro.Trim()));
                }


                if (correo != null)
                {
                    query = query.Where(e => e.CorreoElectronico.Contains(correo.Trim()));
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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Iniciamos la transacción solo en _context, igual que en CreateEmpleadosAsync
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Buscar empleado existente
                var empleado = await _context.Empleado
                    .FirstOrDefaultAsync(e => e.EmpleadoId == dto.EmpleadoId);

                if (empleado == null)
                    throw new Exception("Empleado no existe.");

                // Validar unicidad de documento
                var existeDocumento = await _context.Empleado
                    .AnyAsync(e => e.Documento.ToLower() == dto.Documento.ToLower() && e.EmpleadoId != dto.EmpleadoId);

                if (existeDocumento)
                    throw new Exception("Ya existe un empleado con ese documento.");

                // Actualizar propiedades del Empleado
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

                _context.Empleado.Update(empleado);
                await _context.SaveChangesAsync(); // Guardamos el primer contexto

                // Lógica de Usuario: Buscar si ya existe o crear uno nuevo
                if (dto.Usuario != null)
                {
                    var usuario = await _milucDbContext.Usuarios
                        .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Usuario.Email.ToLower());

                    if (usuario == null)
                    {
                        // Crear nuevo usuario si no existe
                        if (string.IsNullOrWhiteSpace(dto.Usuario.Password))
                            throw new InvalidOperationException("La contraseña es obligatoria para un usuario nuevo.");

                        using var hmac = new System.Security.Cryptography.HMACSHA512();

                        if (usuario.Email != dto.Usuario.Email)
                        {
                            await _emailService.SendAsync(
                                 dto.Usuario.Email,
                                "Bienvenido a Miluc",
                                $"Su cuenta ha sido creada exitosamente ingrese a la platforma: <b>{"https://localhost:7198/login"}</b>. su usuarios {dto.Usuario.Email} su contraseña es  {dto.Usuario.Password} .",
                                true);
                        }

                        usuario = new Usuario
                        {
                            UserName = dto.Usuario.Email,
                            Nombres = dto.Usuario.Nombres,
                            Apellidos = dto.Usuario.Apellidos,
                            Email = dto.Usuario.Email,
                            Telefono = dto.Usuario.Telefono,
                            Foto = dto.Usuario.Foto,
                            TwoFactorEnabled = dto.Usuario.TwoFactorEnabled,
                            DebeCambiarPassword = true,
                            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Usuario.Password)),
                            Salt = hmac.Key,
                            Activo = true,
                            HoraInicio = dto.Usuario.HoraInicio,
                            HoraFin = dto.Usuario.HoraFin,
                            CodVendedorSAP = dto.Usuario.CodVendedorSAP ?? -1,
                            FechaCreacion = DateTime.Now,
                            FechaActualizacion = DateTime.Now
                        };

                        await _milucDbContext.Usuarios.AddAsync(usuario);
                        await _milucDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Actualizar datos básicos del usuario existente si corresponde
                        usuario.Nombres = dto.Usuario.Nombres;
                        usuario.Apellidos = dto.Usuario.Apellidos;
                        usuario.Telefono = dto.Usuario.Telefono;
                        usuario.HoraInicio = dto.Usuario.HoraInicio;
                        usuario.HoraFin = dto.Usuario.HoraFin;
                        usuario.CodVendedorSAP = dto.Usuario.CodVendedorSAP ?? usuario.CodVendedorSAP;
                        usuario.FechaActualizacion = DateTime.Now;

                        // Si viene contraseña nueva, actualizar hash
                        if (!string.IsNullOrWhiteSpace(dto.Usuario.Password))
                        {
                            using var hmac = new System.Security.Cryptography.HMACSHA512();
                            usuario.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Usuario.Password));
                            usuario.Salt = hmac.Key;
                        }

                        _milucDbContext.Usuarios.Update(usuario);
                        await _milucDbContext.SaveChangesAsync();
                    }

                    // Actualizar Roles (Limpiar e Insertar)
                    if (dto.Usuario.RolesIds != null)
                    {
                        var rolesExistentes = _milucDbContext.UsuarioRoles.Where(r => r.IdUsuario == usuario.IdUsuario);
                        _milucDbContext.UsuarioRoles.RemoveRange(rolesExistentes);

                        if (dto.Usuario.RolesIds.Any())
                        {
                            var userRoles = dto.Usuario.RolesIds.Select(rolId => new UsuarioRol
                            {
                                IdUsuario = usuario.IdUsuario,
                                IdRol = rolId
                            });
                            await _milucDbContext.UsuarioRoles.AddRangeAsync(userRoles);
                        }
                    }

                    // Actualizar Tipos de Usuario (Limpiar e Insertar)
                    if (dto.Usuario.TiposUsuarioIds != null)
                    {
                        var tiposExistentes = _milucDbContext.UsuarioTipoUsuario.Where(t => t.IdUsuario == usuario.IdUsuario);
                        _milucDbContext.UsuarioTipoUsuario.RemoveRange(tiposExistentes);

                        if (dto.Usuario.TiposUsuarioIds.Any())
                        {
                            var userTipos = dto.Usuario.TiposUsuarioIds.Select(tipoId => new UsuarioTipoUsuario
                            {
                                IdUsuario = usuario.IdUsuario,
                                IdTipoUsuario = tipoId
                            });
                            await _milucDbContext.UsuarioTipoUsuario.AddRangeAsync(userTipos);
                        }
                    }

                    // Guardar los cambios de las tablas intermedias
                    await _milucDbContext.SaveChangesAsync();
                }




                // Commit global para el contexto de Empleado
                await transaction.CommitAsync();

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
                    FechaCreacion = empleado.FechaCreacion,
                    FechaActualizacion = empleado.FechaActualizacion,
                    Activo = empleado.Activo
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error actualizando empleado: {ex.Message}", ex);
            }
        }
    }
}


