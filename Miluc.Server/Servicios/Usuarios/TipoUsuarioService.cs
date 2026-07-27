using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Server.Models;
using Miluc.Shared.DTOs.Usuarios;

namespace Miluc.Server.Servicios.Usuarios
{
    public class TipoUsuarioService(MilucDbContext _context) : ITipoUsuario
    {
        public async Task<bool> CreateTipoUsuarioAsync(TipoUsuarioCreateDto dto)
        {
            try
            {
                //validamos para evitar duplicados
                var existe = await _context.TipoUsuario
                   .AnyAsync(t => t.Nombre.ToLower() == dto.Nombre.ToLower());
                if (existe)
                    throw new Exception("Ya existe un tipo de usuario con ese nombre");

                TipoUsuario tipoUsuario = new TipoUsuario
                {
                    Nombre = dto.Nombre,
                    Activo = dto.Activo,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow

                };

                await _context.TipoUsuario.AddAsync(tipoUsuario);
               return await _context.SaveChangesAsync()>0;

            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrándola o devolviendo un error específico
                throw new Exception("Error al crear el tipo de usuario", ex);

            }
        }

        public async Task<bool> DeleteTipoUserAsync(int idUsuario)
        {
            try
            {
                var tipoUsuario = await _context.TipoUsuario.FindAsync(idUsuario);
              
                if(tipoUsuario==null)
                    return false;

                tipoUsuario.Activo = false;
                tipoUsuario.FechaActualizacion = DateTime.UtcNow;

                return  await _context.SaveChangesAsync()>0;
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrándola o devolviendo un error específico
                throw new Exception("Error al eliminar el tipo de usuario", ex);
            }
        }

        public async Task<List<TipoUsuarioReadDto>> GetAllTipoUsuarioAsync()
        {
            try
            {

                return await _context.TipoUsuario.AsNoTracking().Where(x=>x.Activo==true)
                    .OrderByDescending(x=>x.FechaCreacion)
                    .Select(x=>new TipoUsuarioReadDto
                    {
                        IdTipoUsuario=x.IdTipoUsuario,
                        Nombre=x.Nombre,
                        Activo=x.Activo,
                        FechaCreacion=x.FechaCreacion

                    })
                    .ToListAsync();

                //return ListaTipoUsuario;

            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrándola o devolviendo un error específico
                throw new Exception("Error al obtener los tipos de usuario", ex);
            }
        }

        public async Task<TipoUsuarioReadDto?> GetByIdTipoUsuarioAsync(int id)
        {
            try
            {
                var tipoUsuario = await _context.TipoUsuario.AsNoTracking().Where(t=>t.Activo==true)
                    .Select(t=>new TipoUsuarioReadDto
                    {
                        IdTipoUsuario=t.IdTipoUsuario,
                        Nombre=t.Nombre,
                        Activo=t.Activo,
                        FechaCreacion=t.FechaCreacion

                    })
                    .FirstOrDefaultAsync(x => x.IdTipoUsuario == id);

                return tipoUsuario;
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrándola o devolviendo un error específico
                throw new Exception("Error al obtener el tipo de usuario por ID", ex);
            }
        }

        public async Task<bool?> UpdateTipoUsuarioAsync(TipoUsuarioUpdateDto dto)
        {
            try
            {
                var tipo = await _context.TipoUsuario.FindAsync(dto.IdTipoUsuario);

                if (tipo == null) return null;

                tipo.Nombre = dto.Nombre;
                tipo.Activo = dto.Activo;
                tipo.FechaActualizacion = DateTime.UtcNow;

                return await _context.SaveChangesAsync() > 0;


            }
            catch(Exception ex)
            {
                throw new Exception("Error al actualizar el tipo de usuario", ex);
            }
        }
    }
}
