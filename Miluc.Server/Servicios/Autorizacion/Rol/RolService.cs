using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Roles;
using Miluc.Server.Models;
using Miluc.Shared.DTOs.Roles;
using MimeKit.Cryptography;
namespace Miluc.Server.Servicios.Autorizacion.Rol
{
    public class RolService(MilucDbContext _context) : IRolService
    {
        public async Task<bool> CreateRolAsync(RolCreateDto dtoCreate)
        {
            // Validación de negocio: No permitir nombres duplicados Insensible a mayúsculas
            if (await _context.Roles.AnyAsync(r => r.Nombre.ToLower() == dtoCreate.Nombre.ToLower()))
            {
                throw new Exception($"Intento crear un rol duplicado {dtoCreate.Nombre}");
                //_logger.LogWarning("Intento de crear un rol duplicado: {Nombre}", dtoCreate.Nombre);
           
            }
            try
            {
                Roles rol = new Roles
                {
                    Nombre = dtoCreate.Nombre,
                    Descripcion = dtoCreate.Descripcion,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                };
                await _context.Roles.AddAsync(rol);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "Error al crear un nuevo rol: {Nombre}", dtoCreate.Nombre);
                throw new Exception($"Error al crear un nuevo rol: {dtoCreate.Nombre}");
            }
        }
        public async Task<bool> DeleteRolAsync(int idRole)
        {
            // Usamos transacción para asegurar que el borrado lógico sea atómico
            //using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var rol = await _context.Roles.Where(x=>x.IdRol==idRole).FirstOrDefaultAsync();


                if (rol==null)
                {
                    return false;
                    
                }

                _context.Roles.Remove(rol);

                return await _context.SaveChangesAsync()>0;
                //var rol = await _context.Roles.FindAsync(idRole);
                //if (rol == null) return false;
                //// Validación de integridad: No borrar roles con usuarios vinculados
                //bool tieneUsuarios = await _context.UsuarioRoles.AnyAsync(ur => ur.IdRol == idRole);
                //if (tieneUsuarios)
                //{

                //    return false;
                //}
                //// Borrado Lógico según tu sugerencia
                //rol.Activo = false;
                //rol.FechaActualizacion = DateTime.Now;
                //_context.Roles.Update(rol);
                //if (await _context.SaveChangesAsync() > 0)
                //{
                //    await transaction.CommitAsync();
                //    return true;
                //}

                //await transaction.RollbackAsync();
                //return false;
            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex, "Error al eliminar el rol con Id: {idRole}", idRole);
                //await transaction.RollbackAsync();
                throw new Exception($"Error al eliminar el rol {ex.Message}");

            }
        }
        public async Task<(List<RolReadDto> data, int totalRegistros)> GetAllRolesAsync(string? buscar = null, int? pagina = null, int? cantidad = null)
        {
            try
            {
                var queryBusqueda = _context.Roles.AsNoTracking().AsQueryable();



                if (!string.IsNullOrEmpty(buscar))
                {
                    queryBusqueda = queryBusqueda.Where(r => r.Nombre.Contains(buscar) || (r.Descripcion.Contains(buscar)));
                }
                int totalRegistros = await queryBusqueda.CountAsync();

                // AsNoTracking mejora el rendimiento en listas grandes
                if (pagina.HasValue && cantidad.HasValue)
                {
                    if (pagina.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(pagina), "El numero de la pagina no puede ser negativo");

                    if (cantidad.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de registras debe ser mayor  a cero");

                    queryBusqueda = queryBusqueda.OrderBy(u => u.IdRol).Skip((pagina.Value - 1) * cantidad.Value).Take(cantidad.Value);
                }

                var dataBusqueda = await queryBusqueda.Select(
                    r => new RolReadDto
                    {
                        IdRol = r.IdRol,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Activo = r.Activo,
                        CantidadRoles = _context.UsuarioRoles.Count(ur => ur.IdRol == r.IdRol)
                    }).ToListAsync();


                return (dataBusqueda, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar Usuarios: {ex.Message}");

             
            }
        }
        //visualizar el detalle de los roles 
        public async Task<RolReadDto?> GetByIdRolesAsync(int idRole)
        {
            try
            {

                var roles = await _context.Roles
                    .AsNoTracking()
                    .Where(r => r.IdRol == idRole)
                    .Select(r => new RolReadDto
                    {
                        IdRol = r.IdRol,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Activo = r.Activo,
                        CantidadRoles = _context.UsuarioRoles.Count(ur => ur.IdRol == r.IdRol)
                    }).FirstOrDefaultAsync();

                return roles;
            }
            catch (Exception ex)
            { 
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<bool> UpdateRolAsync(RolUpdateDto dtoUpdate)
        {
            try
            {
                var rol = await _context.Roles.FindAsync(dtoUpdate.IdRol);
                if (rol == null) return false;

                // Validación de nombre duplicado al editar
                if (rol.Nombre.ToLower() != dtoUpdate.Nombre.ToLower())
                {
                    bool existe = await _context.Roles.AnyAsync(r => r.Nombre.ToLower() == dtoUpdate.Nombre.ToLower() && r.IdRol != dtoUpdate.IdRol);
                    if (existe) return false;
                }

                rol.Nombre = dtoUpdate.Nombre.Trim();
                rol.Descripcion = dtoUpdate.Descripcion.Trim();
                rol.Activo = dtoUpdate.Activo;
                rol.FechaActualizacion = DateTime.Now;
                _context.Roles.Update(rol);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el rol con Id:{ex.Message}");
            }
        }

    }
}
