using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Server.Models;
using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Servicios.Usuarios
{
    public class FiltrosUsuariosService(MilucDbContext _context) : IFiltrosUsuarios
    {


        public async Task<(List<AsignarFiltrosReaderDto> Data, int TotalRegistros)> GetFiltrosAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                var query = _context.AsignarFiltros.Include(x => x.Usuario)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(x =>
                        x.NombreModulo.Contains(filtro) ||
                        x.Descripcion.Contains(filtro) ||
                        x.Filtro.Contains(filtro));
                }

                int totalRegistros = await query.CountAsync();

                var filtros = await query
                    .Select(x => new AsignarFiltrosReaderDto
                    {
                        AsignarFiltrosId = x.AsignarFiltrosId,
                        IdUsuario = x.IdUsuario,
                        NombreModulo = x.NombreModulo,
                        NombreUsuario = x.Usuario.UserName,
                        Descripcion = x.Descripcion,
                        Filtro = x.Filtro
                    })
                    .OrderBy(x => x.AsignarFiltrosId)
                    .Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .ToListAsync();

                return (filtros, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los filtros: {ex.Message}");
            }
        }

        public async  Task<AsignarFiltrosReaderDto> GetByIdFiltrosAsync(int idFiltros)
        {
            try
            {
                var filtro =await _context.AsignarFiltros.AsNoTracking().Include(x=>x.Usuario)
                    .FirstOrDefaultAsync(x => x.AsignarFiltrosId == idFiltros);
                if (filtro == null)
                {
                    throw new Exception($"No se encontró el filtro con Id {idFiltros}");
                }
                var dto = new AsignarFiltrosReaderDto
                {
                    AsignarFiltrosId = filtro.AsignarFiltrosId,
                    IdUsuario = filtro.IdUsuario,
                    NombreModulo = filtro.NombreModulo,
                    NombreUsuario = filtro.Usuario.UserName,
                    Descripcion = filtro.Descripcion,
                    ///*NombrePermiso*/ = filtro.Usuario.UsuarioRoles.Select(x => x.Rol).SelectMany(x => x.RolPermisos).Select(x => x.Permiso).FirstOrDefault(x=>x.Nombre==filtro.NombreModulo).Nombre,
                    Filtro = filtro.Filtro
                };

                //var permiso = await _context.Permisos.AsNoTracking().FirstOrDefaultAsync(x => x.Nombre.ToString().ToUpper() == dto.NombreModulo.ToString().ToUpper());

                //dto.NombrePermiso = permiso.Nombre;
                // return Task.FromResult(dto);

                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el filtro: {ex.Message}");
            }
        }

        public async Task<bool> CreateFiltrosAsync(AsignarFiltrosCreateDto dtoCreate)
        {
            try { 
                
                

                if (dtoCreate == null)
                {
                    throw new Exception("No se pudo crear el filtro");
                }

                AsignarFiltros filtro = new AsignarFiltros
                {
                    IdUsuario = dtoCreate.IdUsuario,
                    NombreModulo = dtoCreate.NombreModulo,
                    Descripcion = dtoCreate.Descripcion,
                    Filtro = dtoCreate.Filtro
                };
               
                _context.AsignarFiltros.Add(filtro);
               
                return await _context.SaveChangesAsync()>0;
                



            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el filtro: {ex.Message}");
            }
        }

      

      

        public async Task<bool> UpdateFiltrosAsync(AsignarFiltroUpdateDto dtoUpdate)
        {
            try { 
                var updateFiltro = _context.AsignarFiltros.FirstOrDefault(x => x.AsignarFiltrosId == dtoUpdate.AsignarFiltrosId);

                
                if (updateFiltro == null)
                {
                    throw new Exception($"No se encontró el filtro con Id {dtoUpdate.AsignarFiltrosId}");
                }
                updateFiltro.IdUsuario = dtoUpdate.IdUsuario;
                updateFiltro.NombreModulo = dtoUpdate.NombreModulo;
                updateFiltro.Descripcion = dtoUpdate.Descripcion;
                updateFiltro.Filtro = dtoUpdate.Filtro;



                return await _context.SaveChangesAsync() > 0;



            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el filtro: {ex.Message}");
            }
        }


        public async Task<bool> DeleteFiltrosAsync(int idFiltros)
        {
            try
            {
                var filtro = _context.AsignarFiltros.FirstOrDefault(x => x.AsignarFiltrosId == idFiltros);
                if (filtro == null)
                {
                    throw new Exception($"No se encontró el filtro con Id {idFiltros}");
                }
                _context.AsignarFiltros.Remove(filtro);
               return await _context.SaveChangesAsync()>0;
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el filtro: {ex.Message}");
            }
        }

        public async Task<(List<PermisosReadDto> Data, int TotalRegistros)> GetAllPermisosModuloAsyncPage(string? buscar = null, int pagina = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                // 1. Definimos la query base con los joins necesarios
                var queryBusqueda = _context.Permisos
                    .AsNoTracking().Where(r =>
                     r.Nombre.Contains("Modulo.")).AsQueryable();

                    
                

                // 2. Aplicamos el filtro solo si "buscar" tiene contenido
                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    // Usamos ToLower() o dejamos que el Collation de SQL decida (case-insensitive)
                    queryBusqueda = queryBusqueda.Where(r =>
                        r.Nombre.Contains(buscar)  ||
                        (r.Descripcion != null && r.Descripcion.Contains(buscar)) );
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

        //public async Task<List<string>> GetModulosAsync()
        //{
        //    try
        //    {
        //        return await _context.Permisos
        //            .AsNoTracking()
        //            .Where(x => x.Nombre.Contains("Modulo."))
        //            .Select(x => x.Nombre)
        //            .OrderBy(x => x)
        //            .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al obtener los módulos: {ex.Message}");
        //    }
        //}


    }


    }



