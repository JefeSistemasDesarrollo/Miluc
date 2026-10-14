using Microsoft.EntityFrameworkCore;
using Miluc.Client.Interfaces.Nomina;
using Miluc.Server.Data;
using Miluc.Server.Models.Nomina;

using Miluc.Shared.DTOs.Nomina.CargosDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class CargoService(NominaDbContext _context) : ICargoService
    {
        

        public async Task<(List<CargosReaderDto> Data, int TotalRegistros)> GetCargosAsync(string? filtro = null, int page = 1,int? cantidad = null,string? correo = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;
                filtro ??= string.Empty;

                var query = _context.Cargos.AsNoTracking().AsQueryable();


                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string filtroMinuscula = filtro.Trim().ToLower();
                    query = query.Where(c => c.CargoNombre.ToLower().Contains(filtroMinuscula));
                }


                int totalRegistros = await query.CountAsync();

            
                var cargos = await query
                    .Select(c => new CargosReaderDto
                    {
                        CargoId = c.CargoId,
                        CargoNombre = c.CargoNombre,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion =DateTime.Now,
                        Activo = c.Activo
                    }).OrderBy(c => c.CargoNombre )
                    .Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .ToListAsync();

                return (cargos, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener cargos: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpsertCargoAsync(UpsertCargos cargoUpdateDto)
        {
            try
            {
                var nombreCargo = cargoUpdateDto.CargoNombre.Trim().ToUpper();

                var cargoExistente = await _context.Cargos
                    .FirstOrDefaultAsync(c => c.CargoId == cargoUpdateDto.CargoId);

                if (cargoExistente == null)
                {
                    // Validar nombre duplicado
                    bool existeNombre = await _context.Cargos
                        .AnyAsync(c => c.CargoNombre == nombreCargo);

                    if (existeNombre)
                        throw new ArgumentException("Ya existe un cargo con ese nombre.");

                    var newCargo = new Cargo
                    {
                        CargoNombre = nombreCargo,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Activo = cargoUpdateDto.Activo
                    };

                    await _context.Cargos.AddAsync(newCargo);
                }
                else
                {
                    // Validar que otro registro no tenga ese nombre
                    bool existeNombre = await _context.Cargos.AnyAsync(c =>
                        c.CargoId != cargoUpdateDto.CargoId &&
                        c.CargoNombre == nombreCargo);

                    if (existeNombre)
                        throw new ArgumentException("Ya existe un cargo con ese nombre.");

                    cargoExistente.CargoNombre = nombreCargo;
                    cargoExistente.FechaActualizacion = DateTime.Now;
                    cargoExistente.Activo = cargoUpdateDto.Activo;
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var valorReal = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Error al guardar el cargo: {valorReal}");
            }
        }
        public async Task<CargosReaderDto> GetCargoByIdAsync(int cargoId)
        {
            var cargo = await _context.Cargos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CargoId == cargoId);

            if (cargo == null)
                throw new InvalidOperationException("Cargo no encontrado");

            return new CargosReaderDto
            {
                CargoId = cargo.CargoId,
                CargoNombre = cargo.CargoNombre,
                FechaCreacion = cargo.FechaCreacion,
                FechaActualizacion = cargo.FechaActualizacion,
                Activo = cargo.Activo
            };
        }


        public async Task<bool> DeleteCargoAsync(int cargoId)
        {
            try
            {
                var cargo = await _context.Cargos.FindAsync(cargoId);

                if (cargo == null)
                {
                    throw new Exception($"No se encontró el cargo con ID {cargoId}");
                }

                var empleadosAsociados = await _context.ContratoLaboralDetalle
                    .CountAsync(x => x.CargoId == cargoId);

                if (empleadosAsociados > 0)
                {
                    throw new Exception(
                        $"No se puede eliminar el cargo porque está siendo utilizado por {empleadosAsociados} empleado(s).");
                }

                _context.Cargos.Remove(cargo);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    }
