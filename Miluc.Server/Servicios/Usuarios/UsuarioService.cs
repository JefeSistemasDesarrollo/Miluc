using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Server.Models;
using Miluc.Shared.DTOs.Usuarios;

namespace Miluc.Server.Servicios.Usuarios
{
    public class UsuarioService(MilucDbContext _context) : IUsuarioService
    {
        public async Task<UsuarioReadDto> CreateUsuarioAsync(UsuarioCreateDto dto)
        {

            if(dto==null)
                throw new ArgumentNullException("El objeto del usuario viene vacío",nameof(dto));
         
            // 1. Validaciones de existencia (Username y Email)
            if (await _context.Usuarios.AnyAsync(u => u.UserName.ToLower() == dto.UserName.ToLower()))
                throw new InvalidOperationException("El nombre de usuario ya está registrado.");

            if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                throw new InvalidOperationException("El correo electrónico ya está en uso.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new InvalidOperationException("La contraseña es obligatoria");

            // 2. Iniciamos Transacción
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                using var hmac = new System.Security.Cryptography.HMACSHA512();

                // 3. Mapeo de Entidad Principal
                var usuario = new Usuario
                {
                    UserName = dto.UserName,
                    Nombres = dto.Nombres,  
                    Apellidos = dto.Apellidos,
                    Email = dto.Email,
                    Telefono = dto.Telefono,
                    Foto = dto.Foto,
                    TwoFactorEnabled=true,
                    DebeCambiarPassword = dto.DebeCambiarPassword,
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Password)),
                    Salt = hmac.Key,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                };


                await _context.Usuarios.AddAsync(usuario);
                // Guardamos para generar el IdUsuario
                await _context.SaveChangesAsync();

                // 4. Asignación de Roles (Tabla Intermedia)
                if (dto.RolesIds != null && dto.RolesIds.Any())
                {
                    var userRoles = dto.RolesIds.Select(rolId => new UsuarioRol
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdRol = rolId
                    });
                    await _context.UsuarioRoles.AddRangeAsync(userRoles);
                }

                // 5. Asignación de Tipos de Usuario (Tabla Intermedia)
                if (dto.TiposUsuarioIds != null && dto.TiposUsuarioIds.Any())
                {
                    var userTipos = dto.TiposUsuarioIds.Select(tipoId => new UsuarioTipoUsuario
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdTipoUsuario = tipoId
                    });
                    await _context.UsuarioTipoUsuario.AddRangeAsync(userTipos);
                }

                // Guardamos las relaciones
                await _context.SaveChangesAsync();

                // 6. Confirmamos cambios
                await transaction.CommitAsync();

                // 7. Retornamos el DTO de lectura completo
                return new UsuarioReadDto
                {
                    IdUsuario = usuario.IdUsuario,
                    UserName = usuario.UserName,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Email = usuario.Email,
                    Activo = usuario.Activo
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Es mejor relanzar la excepción original o una personalizada con el mensaje interno
                throw new Exception($"Error interno al crear usuario: {ex.Message}", ex);
            }
        }
        public async Task<UsuarioReadDto?> UpdateUsuarioAsync(UsuarioUpdateDto dto)
        {

            if (dto == null)
                throw new ArgumentNullException("El objeto del usuario viene vacío", nameof(dto));


            // 1. Validar que el usuario exista
            var user = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .Include(u => u.UsuarioTipoUsuario)
                .FirstOrDefaultAsync(u => u.IdUsuario == dto.IdUsuario);

            if (user == null)     
                throw new ArgumentNullException("", nameof(user));


            // 2. Validar que el nuevo Email no lo tenga otro usuario
            var emailOcupado = await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()
                               && u.IdUsuario != dto.IdUsuario);

            if (emailOcupado)
                throw new Exception("El correo electrónico ya pertenece a otro usuario.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //  datos de usuarios 
                user.Email = dto.Email;
                user.Activo = dto.Activo;
                user.Nombres = dto.Nombres;
                user.Apellidos = dto.Apellidos;
                user.Telefono = dto.Telefono;

                if (dto.Foto != null && dto.Foto.Length > 0)
                {
                    user.Foto = dto.Foto;
                }
                //user.TwoFactorEnabled = dto.TwoFactorEnabled;
                user.Activo = dto.Activo;
                user.TwoFactorEnabled=dto.TwoFactorEnabled;
                user.DebeCambiarPassword = dto.DebeCambiarPassword;
                user.FechaActualizacion = DateTime.UtcNow;
                //  PASSWORD (solo si se envía) 
                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    using var hmac = new System.Security.Cryptography.HMACSHA512();
                    user.Salt = hmac.Key;
                    user.PasswordHash = hmac.ComputeHash(
                        System.Text.Encoding.UTF8.GetBytes(dto.Password)
                    );
                }             
                //  ROLES 
                user.UsuarioRoles.Clear();

                if (dto.RolesIds != null)
                {
                    foreach (var id in dto.RolesIds.Distinct())
                    {
                        user.UsuarioRoles.Add(new UsuarioRol
                        {
                            IdUsuario = user.IdUsuario,
                            IdRol = id
                        });
                    }
                }
                //  TIPOS DE USUARIO 
                user.UsuarioTipoUsuario.Clear();

                if (dto.TiposUsuarioIds != null)
                {
                    foreach (var id in dto.TiposUsuarioIds.Distinct())
                    {
                        user.UsuarioTipoUsuario.Add(new UsuarioTipoUsuario
                        {
                            IdUsuario = user.IdUsuario,
                            IdTipoUsuario = id
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new UsuarioReadDto
                {
                    IdUsuario = user.IdUsuario,
                    UserName = user.UserName
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            try
            {


                //ACA VAMOS ELIMINAR EL USUARIO 
                var deleteUser = await _context.Usuarios.Where(x=>x.IdUsuario==id).FirstOrDefaultAsync();

                if (deleteUser==null)
                {
                    return false;
                }

                _context.Remove(deleteUser);

                 return await _context.SaveChangesAsync()>0;
                //var user = await _context.Usuarios.FindAsync(id);
                //// Si es NULL, no existe, por lo tanto retornamos false
                ////validar si el usuario existe en la base de datos 
                //if (user == null)
                //    return false;

                //user.Activo = false;
                //user.FechaActualizacion = DateTime.Now;

               // return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex) 
            {
              
                throw new Exception($"Error al eliminar el Usuario {ex.Message}");
               
            }
        }
        public async Task<UsuarioReadDto?> GetByIdUsuarioAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El id del usuario debe ser mayor a cero",nameof(id));

                // Importante: Incluimos relaciones y proyectamos al DTO
                var usuarioGetId = await _context.Usuarios
                      .AsNoTracking()
                      .Include(u => u.UsuarioRoles)
                      .ThenInclude(ur => ur.Rol)
                      .Include(u => u.UsuarioTipoUsuario)
                      .ThenInclude(u => u.TipoUsuario)
                      .Where(u => u.IdUsuario == id)
                      .Select(u => new UsuarioReadDto
                      {
                          IdUsuario = u.IdUsuario,
                          UserName = u.UserName,
                          Nombres = u.Nombres,
                          Apellidos = u.Apellidos,
                          Telefono = u.Telefono,
                          Foto = u.Foto,
                          TwoFactorEnabled= u.TwoFactorEnabled,
                          Email = u.Email,
                          Activo = u.Activo,
                          NombresRoles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList(),
                          NombresTiposUsuario = u.UsuarioTipoUsuario.Select(ut => ut.TipoUsuario.Nombre).ToList(),
                          RolesIds = u.UsuarioRoles.Select(ur => ur.Rol.IdRol).ToList(),
                          TiposUsuarioIds = u.UsuarioTipoUsuario.Select(ut => ut.IdTipoUsuario).ToList(),
                      })
                      .FirstOrDefaultAsync();

                return usuarioGetId;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar Usuarios {ex.Message}");

            }
        }
        public async Task<(List<UsuarioReadDto> Data, int TotalRegistros)> GetAllUsuariosAsync(string? buscar = null, int ? pagina = null, int? cantidad = null)
        {
            try
            {

                if (string.IsNullOrEmpty(cantidad?.ToString()))
                {
                    cantidad = 10;
                }

                //  int cantidadTop = cantidad ?? 20;

                var queryBusqueda = _context.Usuarios.AsNoTracking().AsQueryable();
                //.Include(u => u.UsuarioRoles).ThenInclude(r => r.Rol)
                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    // Usamos ToLower() o dejamos que el Collation de SQL decida (case-insensitive)
                    queryBusqueda = queryBusqueda.Where(r =>
                        r.UserName.Contains(buscar) ||
                        (r.Nombres != null && r.Apellidos.Contains(buscar)));
                }

                int totalEncontrados = await queryBusqueda.CountAsync();


                if (pagina.HasValue && cantidad.HasValue)
                {
                    if (pagina.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(pagina), "El numero de la pagina no puede ser negativo");

                    if (cantidad.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de registras debe ser mayor  a cero");
                    
                    queryBusqueda=queryBusqueda.OrderBy(u=>u.IdUsuario).Skip((pagina.Value-1)*cantidad.Value).Take(cantidad.Value);
                }

                var dataBusqueda= await queryBusqueda
                    //.OrderByDescending(x=>x.Nombres)
                    //.Skip((pagina - 1) * cantidadTop)
                    //.Take(cantidadTop)
                    .Select(u => new UsuarioReadDto
                    {
                        IdUsuario = u.IdUsuario,
                        UserName = u.UserName,
                        Nombres = u.Nombres,
                        Apellidos = u.Apellidos,
                        Telefono = u.Telefono,
                        Foto = u.Foto,
                        TwoFactorEnabled= u.TwoFactorEnabled,
                        Email = u.Email,
                        Activo = u.Activo,
                        NombresRoles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList(),
                        NombresTiposUsuario = u.UsuarioTipoUsuario.Select(ut => ut.TipoUsuario.Nombre).ToList()

                    }).ToListAsync();
                return (dataBusqueda, totalEncontrados);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar Usuarios: {ex.Message}");
            }
        }

    }
}
