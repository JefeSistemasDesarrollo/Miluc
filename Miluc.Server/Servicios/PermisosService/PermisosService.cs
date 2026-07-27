using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Permisos;
using Miluc.Server.Models;
using Miluc.Shared.DTOs.Permisos;

namespace Miluc.Server.Servicios.PermisosService
{
    public class PermisosService(MilucDbContext _context) : IPermisosService
    {
        //crear pedidos de los clientes 
        public async Task<bool> CreatePermisosAsync(PermisosCreateDto permisosCreate)
        {
            if (await _context.Permisos
                .AnyAsync(p => p.Nombre.ToLower() == permisosCreate.Nombre.ToLower()))
                throw new Exception("El permiso ya se encuentra registrado");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Permisos permisos = new Permisos
                {
                    Nombre = permisosCreate.Nombre,
                    Descripcion = permisosCreate.Descripcion,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                };

                await _context.Permisos.AddAsync(permisos);
                await _context.SaveChangesAsync();

                if (permisosCreate.RolesIds != null && permisosCreate.RolesIds.Any())
                {
                    // VALIDAR ROLES
                    var roles = await _context.Roles
                        .Where(r => permisosCreate.RolesIds.Contains(r.IdRol))
                        .Select(r => r.IdRol)
                        .ToListAsync();

                    var userPermiso = roles.Select(rolId => new RolPermiso
                    {
                        IdRol = rolId,
                        IdPermiso = permisos.IdPermiso
                    });

                    await _context.RolPermisos.AddRangeAsync(userPermiso);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear el permiso {ex.Message}");
            }
        }
        public async Task<bool> DeletePermisoAsync(int idPermiso)
        {
            try
            {
                var user = await _context.Permisos.FindAsync(idPermiso);
                if (user == null)
                    return false;
                user.Activo = false;
                user.FechaActualizacion = DateTime.Now;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {

                return false;
                throw new Exception($"Error al consultar Usuarios {ex.Message}");
            }
        }
        public async Task<(List<PermisosReadDto> Data, int TotalRegistros)> GetAllPermisosAsyncPage(string? buscar = null, int pagina = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                // 1. Definimos la query base con los joins necesarios
                var queryBusqueda = _context.Permisos
                    .AsNoTracking()
                    .Include(u => u.RolPermisos)
                        .ThenInclude(r => r.Rol)
                    .AsQueryable();

                // 2. Aplicamos el filtro solo si "buscar" tiene contenido
                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    // Usamos ToLower() o dejamos que el Collation de SQL decida (case-insensitive)
                    queryBusqueda = queryBusqueda.Where(r =>
                        r.Nombre.Contains(buscar) ||
                        (r.Descripcion != null && r.Descripcion.Contains(buscar)));
                }

                // 3. Contamos el total antes de paginar (necesario para Blazor)
                int totalEncontrados = await queryBusqueda.CountAsync();

                // 4. Aplicamos orden, salto y toma de registros
                var dataBusqueda = await queryBusqueda
                    .OrderByDescending(r => r.Nombre)
                    .Skip((pagina - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .Select(p => new PermisosReadDto
                    {
                        IdPermiso = p.IdPermiso,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        FechaCreacion = p.FechaCreacion,
                        FechaActualizacion = p.FechaActualizacion,
                        Activo = p.Activo,
                        // Proyectamos directamente los nombres e IDs de los roles
                        RolesIds = p.RolPermisos.Select(rp => rp.Rol.IdRol).ToList(),
                        RolesNombre = p.RolPermisos.Select(rp => rp.Rol.Nombre).ToList(),
                    })
                    .ToListAsync();

                return (dataBusqueda, totalEncontrados);
            }
            catch (Exception ex)
            {
                // Corregido el mensaje de "Usuarios" a "Permisos" para ser precisos
                throw new Exception($"Error al listar Permisos: {ex.Message}");
            }
        }
        public async Task<PermisosReadDto> GetByIdPermisoAsync(int idPermiso)
        {
            try
            {
                var permisoId = await _context.Permisos.AsNoTracking().Include(rp => rp.RolPermisos)
                    .ThenInclude(r => r.Rol).Where(p => p.IdPermiso == idPermiso).
                    Select(p => new PermisosReadDto
                    {
                        IdPermiso = idPermiso,
                        Nombre = p.Nombre,
                        Activo = p.Activo,
                        Descripcion =p.Descripcion,
                        FechaCreacion = p.FechaCreacion,
                        FechaActualizacion = p.FechaActualizacion,
                        RolesNombre = p.RolPermisos.Select(r => r.Rol.Nombre).ToList(),
                        RolesIds=p.RolPermisos.Select(r=>r.Rol.IdRol).ToList()
                    }).FirstOrDefaultAsync();

                return permisoId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar el Permiso: {ex.Message}");
            }
        }
        public async Task<bool> UpdatePermisosAsync(PermisosUpdateDto permisosUpdate)
        {
            var permiso = await _context.Permisos
                .Include(rp => rp.RolPermisos)
                .FirstOrDefaultAsync(p => p.IdPermiso == permisosUpdate.IdPermiso);

            if (permiso == null)
                throw new Exception("El permiso no existe");

            var permisosNombre = await _context.Permisos.AnyAsync(p =>
                p.Nombre.ToLower() == permisosUpdate.Nombre.ToLower()
                && p.IdPermiso != permisosUpdate.IdPermiso);

            if (permisosNombre)
                throw new Exception("El permiso ya está creado");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                permiso.Nombre = permisosUpdate.Nombre;
                permiso.Descripcion = permisosUpdate.Descripcion;
                permiso.FechaActualizacion = permisosUpdate.FechaActualizacion;
                permiso.Activo = permisosUpdate.Activo;

                permiso.RolPermisos.Clear();

                if (permisosUpdate.RolesIds != null && permisosUpdate.RolesIds.Any())
                {
                    foreach (var id in permisosUpdate.RolesIds.Distinct())
                    {
                        permiso.RolPermisos.Add(new RolPermiso
                        {
                            IdPermiso = permiso.IdPermiso,
                            IdRol = id
                        });
                    }
                }

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
                await transaction.RollbackAsync();
                throw new Exception($"Error al actualizar el permiso: {ex.Message}");
            }
        }
        
    }
}
