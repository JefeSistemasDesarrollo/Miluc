using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Roles;
using Miluc.Server.Models;
using Miluc.Shared.DTOs.Roles;
namespace Miluc.Server.Servicios.Autorizacion.Rol
{
    public class RolService(MilucDbContext _context, ILogger<RolService> _logger) : IRolService
    {
        public async Task<bool> CreateRolAsync(RolCreateDto dtoCreate)
        {
            // Validación de negocio: No permitir nombres duplicados Insensible a mayúsculas
            if (await _context.Roles.AnyAsync(r => r.Nombre.ToLower() == dtoCreate.Nombre.ToLower()))
            {
                _logger.LogWarning("Intento de crear un rol duplicado: {Nombre}", dtoCreate.Nombre);
              return false;
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
                return await _context.SaveChangesAsync()>0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo rol: {Nombre}", dtoCreate.Nombre);
                throw;
            }
        }
        public async Task<bool> DeleteRolAsync(int idRole)
        {
            // Usamos transacción para asegurar que el borrado lógico sea atómico
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rol = await _context.Roles.FindAsync(idRole);
                if (rol == null) return false;
                // Validación de integridad: No borrar roles con usuarios vinculados
                bool tieneUsuarios = await _context.UsuarioRoles.AnyAsync(ur => ur.IdRol == idRole);
                if (tieneUsuarios)
                {
                    _logger.LogWarning("No se puede eliminar el rol ID: {idRole} porque tiene usuarios asignados", idRole);
                    return false;
                }
                // Borrado Lógico según tu sugerencia
                rol.Activo = false;
                rol.FechaActualizacion = DateTime.Now;
                _context.Roles.Update(rol);
                if (await _context.SaveChangesAsync() > 0)
                {
                    await transaction.CommitAsync();
                    return true;
                }

                await transaction.RollbackAsync();
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con Id: {idRole}", idRole);
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<RolReadDto>> GetAllRolesAsync()
        {
            try
            {
                // AsNoTracking mejora el rendimiento en listas grandes
                return await _context.Roles
                    .AsNoTracking()
                    .Select(r => new RolReadDto
                    {
                        IdRol = r.IdRol,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Activo = r.Activo,
                        CantidadRoles = _context.UsuarioRoles.Count(ur => ur.IdRol == r.IdRol)
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw;
            }
        }
        //visualizar el detalle de los roles 
        public async Task<RolReadDto?> GetByIdRolesAsync(int idRole)
        {
            try
            {

                var roles= await _context.Roles
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
                _logger.LogError(ex, "Error al obtener el rol con Id: {idRole}", idRole);
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
                return await _context.SaveChangesAsync()>0 ;
                 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con Id: {Id}", dtoUpdate.IdRol);
                throw;
            }
        }

    }
}
