using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class InfoFamiliarService(NominaDbContext _context) : IInfoFamiliarService
    {

        public async Task<bool> CreateFamiliarAsync(List<InfoFamiliarCreateDto> familiar)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {


                foreach (var fam in familiar)
                {
                    var infoFamiliar = new InformacionFamiliar
                    {
                        EmpleadoId = fam.EmpleadoId,
                        Documento = fam.Documento,
                        NombreCompleto = fam.NombreCompleto,
                        ParentescoId = fam.ParentescoId.Value,
                        FechaNacimiento = fam.FechaNacimiento.Value,
                        ViveConEmpleado = fam.ViveConEmpleado.Value,
                        DependeEconomicamente = fam.DependeEconomicamente.Value,
                        PersonaaCargo = fam.PersonaaCargo.Value,
                        Activo = true,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now
                    };
                    await _context.AddAsync(infoFamiliar);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al obtener nuevo empleado:{ex.Message}");

            }
        }

        public async Task<bool> DeleteFamiliarAsync(int id)
        {
            try
            {
                var familiar = await _context.InformacionFamiliar.
                    Where(f => f.InformacionFamiliarId == id).
                    FirstOrDefaultAsync();
                if (familiar == null) 
                    
                    throw new Exception("Familiar no encontrado");
            else 
            {
                _context.InformacionFamiliar.Remove(familiar);
                await _context.SaveChangesAsync();
                return true;
            }
        }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el familiar: {ex.Message}");
            }
        }
        public async Task<List<InfoFamiliarReaderDto>> GetFamiliarByIdAsync(int id)
        {
            try
            {
                var infoFamiliar = await _context.InformacionFamiliar.AsNoTracking()
                    .Include(e => e.Empleado)
                    .Include(e => e.Parentesco)
                    .Where(e => e.EmpleadoId == id)
                    .Select(f => new InfoFamiliarReaderDto
                    {
                        Documento = f.Documento,
                        InformacionFamiliarId = f.InformacionFamiliarId,
                        EmpleadoId = f.EmpleadoId,
                        NombreEmpleado = $"{f.Empleado.PrimerNombre} {f.Empleado.PrimerApellido}",

                        NombreCompleto = f.NombreCompleto,
                        ParentescoId = f.ParentescoId,
                        Parentesco = f.Parentesco.NombreParentesco,

                        FechaNacimiento = f.FechaNacimiento,
                        ViveConEmpleado = f.ViveConEmpleado,
                        DependeEconomicamente = f.DependeEconomicamente,
                        PersonaaCargo = f.PersonaaCargo,
                        Activo = f.Activo,
                    })
                    .ToListAsync();
                if (infoFamiliar == null) throw new Exception("Informacion familiar no encontrada");

                return infoFamiliar;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener nuevo empleado:{ex.Message}");
            }
        }

        public async Task<(List<InfoFamiliarReaderDto> data, int CantidadRegistros)> GetFamiliarListAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadtop = cantidad ?? 20;

                //    var queryinformacion = await _context.InformacionFamiliar.ToListAsync();
                var query = _context.InformacionFamiliar.AsNoTracking().AsQueryable();



                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(x => x.Empleado.PrimerNombre.Contains(filtro) || x.Empleado.PrimerApellido.Contains(filtro));
                }
                var totalRegistros = query.Count();

                var familiares = await query
                    .Include(f => f.Empleado)
                    .Include(f => f.Empleado.InformacionFamiliar)
                    .Include(f => f.Parentesco)
                    .Where(f => f.EmpleadoId == 6)
                    .Select(f => new InfoFamiliarReaderDto
                    {
                        InformacionFamiliarId = f.InformacionFamiliarId,
                        NombreEmpleado = $"{f.Empleado.PrimerNombre} {f.Empleado.SegundoNombre} {f.Empleado.PrimerApellido} {f.Empleado.SegundoApellido}",
                        EmpleadoId = f.EmpleadoId,
                        NombreCompleto = f.NombreCompleto,
                        ParentescoId = f.ParentescoId,
                        Parentesco = f.Parentesco.NombreParentesco,
                        FechaNacimiento = f.FechaNacimiento,
                        ViveConEmpleado = f.ViveConEmpleado,
                        DependeEconomicamente = f.DependeEconomicamente,
                        PersonaaCargo = f.PersonaaCargo
                    })
                    .Skip((page - 1) * cantidadtop)
                    .Take(cantidadtop)
                    .ToListAsync();

                return (familiares, totalRegistros);
            }


            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de familiares: {ex.Message}");
            }
        }


        public async Task<InfoFamiliarReaderDto> UpdateFamiliarAsync(InfoFamiliarUpdateDto dto)
        {
            try
            {
                // 1. BUSCAR EL FAMILIAR ESPECÍFICO A EDITAR
                var familiar = await _context.InformacionFamiliar
                    .FirstOrDefaultAsync(f => f.InformacionFamiliarId == dto.InformacionFamiliarId);

                if (familiar == null)
                    throw new Exception("El registro de información familiar no existe.");

                // 2. VALIDAR QUE EL NOMBRE NO SE DUPLIQUE CON OTRO FAMILIAR DEL MISMO EMPLEADO
                var existeFamiliar = await _context.InformacionFamiliar
                    .AnyAsync(f => f.NombreCompleto.ToUpper() == dto.NombreCompleto.ToUpper()
                                   && f.EmpleadoId == dto.EmpleadoId
                                   && f.InformacionFamiliarId != dto.InformacionFamiliarId);

                if (existeFamiliar)
                    throw new Exception("Ya existe un familiar registrado con ese nombre para este empleado.");

                // 3. ACTUALIZAR LOS CAMPOS EN LA ENTIDAD
                familiar.NombreCompleto = dto.NombreCompleto.ToUpper();
                familiar.ParentescoId = dto.ParentescoId;
                familiar.FechaNacimiento = dto.FechaNacimiento;
                familiar.ViveConEmpleado = dto.ViveConEmpleado.Value;
                familiar.DependeEconomicamente = dto.DependeEconomicamente.Value;
                familiar.PersonaaCargo = dto.PersonaaCargo.Value;
                familiar.Activo = dto.Activo;
                familiar.FechaActualizacion = dto.FechaActualizacion;

                // Guardamos los cambios del familiar editado en la Base de Datos
                await _context.SaveChangesAsync();



                return new InfoFamiliarReaderDto
                {
                    InformacionFamiliarId =familiar.InformacionFamiliarId,
                    Documento = familiar.Documento,
                    NombreCompleto = familiar.NombreCompleto,
                    ParentescoId = familiar.ParentescoId,
                    FechaNacimiento = familiar.FechaNacimiento,
                    ViveConEmpleado = familiar.ViveConEmpleado,
                    DependeEconomicamente = familiar.DependeEconomicamente,
                    PersonaaCargo = familiar.PersonaaCargo,
                    Activo = familiar.Activo,
                    
                    
              









                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la información familiar: {ex.Message}");
            }
        }
    }
}