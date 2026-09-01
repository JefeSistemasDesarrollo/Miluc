using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.Cargos;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Servicios.Nomina
{
    // Servicio encargado de gestionar las operaciones de contratos laborales usando Inyección de Dependencias 
    public class ContratoLaboralService(NominaDbContext _context) : IContratoLaboralService
    {
        // Obtiene una lista paginada y filtrada de contratos laborales asociados a los empleados
        public async Task<(List<ContratoLaboralreaderDto> data, int CantidadDeRegistros)> GetContratoLaboralAsync(string? filtro = null, int? page = null, int? cantidad = null)
        {
            try
            {
                int cantidadtop = cantidad ?? 20;
                int paginador = page ?? 1;

                var query = _context.Empleado.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {
                    string filtroMinuscula = filtro.Trim().ToLower();
                    query = query.Where(x => x.PrimerNombre.ToLower().Contains(filtroMinuscula)
                                          || x.PrimerApellido.ToLower().Contains(filtroMinuscula)
                                          || x.Documento.ToLower().Contains(filtroMinuscula));
                }

                var totalRegistros = await query.CountAsync();

                var datosIntermedios = await query
                    .Skip((paginador - 1) * cantidadtop)
                    .Take(cantidadtop)
                    .Select(x => new
                    {
                        x.EmpleadoId,
                        x.PrimerNombre,
                        x.SegundoNombre,
                        x.PrimerApellido,
                        x.SegundoApellido,
                        x.Documento,

                        Contrato = x.ContratoLaboral
                            .OrderByDescending(c => c.FechaCreacion)
                            .Select(c => new
                            {
                                c.ContratoLaboralId,
                                c.FechaCreacion,
                                c.Activo,
                                c.EmpresaId,

                                NombreEmpresa = c.Empresa != null ? c.Empresa.NombreEmpresa : null,
                                Nit = c.Empresa != null ? c.Empresa.Nit : null,

                                c.TipoContratoId,
                                NombreTipoContrato = c.TipoContrato != null ? c.TipoContrato.NombreContrato : null,

                                Detalle = c.ContratoLaboralDetalle
                                    .OrderByDescending(d => d.FechaInicio)
                                    .Select(d => new
                                    {
                                        d.ContratoLaboralDetalleId,
                                        d.FechaInicio,
                                        d.FechaFinalizacion,
                                        d.FechaTerminacion,
                                        d.CargoId,
                                        NombreCargo = d.Cargo != null ? d.Cargo.CargoNombre : null,
                                        d.CentroCosto,
                                        d.Salario,
                                        d.Observacion
                                    })
                                    .FirstOrDefault()
                            })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                var contratos = datosIntermedios.Select(x => new ContratoLaboralreaderDto
                {
                    EmpleadoId = x.EmpleadoId,
                    NombreEmpleado = $"{x.PrimerNombre} {x.SegundoNombre} {x.PrimerApellido} {x.SegundoApellido}".Replace("  ", " ").Trim(),
                    Documento = x.Documento,

                    ContratoLaboralId = x.Contrato?.ContratoLaboralId ?? 0,
                    FechaCreacion = x.Contrato?.FechaCreacion ?? DateTime.MinValue,

                    EmpresaId = x.Contrato?.EmpresaId ?? 0,
                    NombreEmpresa = x.Contrato?.NombreEmpresa ?? "Sin Asignar",
                    Nit = x.Contrato?.Nit ?? "N/A",

                    TipoContratoId = x.Contrato?.TipoContratoId ?? 0,
                    NombreTipoContrato = x.Contrato?.NombreTipoContrato ?? "Sin Asignar",

                    ContratoLaboralDetalleId = x.Contrato?.Detalle?.ContratoLaboralDetalleId ?? 0,
                    FechaInicio = x.Contrato?.Detalle?.FechaInicio ?? DateTime.MinValue,
                    FechaFinalizacion = x.Contrato?.Detalle?.FechaFinalizacion,
                    FechaTerminacion = x.Contrato?.Detalle?.FechaTerminacion,

                    CargoId = x.Contrato?.Detalle?.CargoId ?? 0,
                    NombreCargo = x.Contrato?.Detalle?.NombreCargo ?? "Sin Asignar",

                    CentroCosto = x.Contrato?.Detalle?.CentroCosto,
                    Salario = x.Contrato?.Detalle?.Salario ?? 0,
                    Observacion = x.Contrato?.Detalle?.Observacion,

                    Activo = x.Contrato?.Activo ?? false
                }).ToList();

                return (contratos, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener contratos laborales: {ex.Message}", ex);
            }
        }

        // NUEVO: Obtiene un contrato laboral único buscando directamente por su ContratoLaboralId (Para la pantalla de Editar)
        public async Task<ContratoLaboralreaderDto?> GetContratoByIdAsync(int contratoLaboralId)
        {
            try
            {
                var contrato = await _context.ContratoLaboral.AsNoTracking()
                    .Where(c => c.ContratoLaboralId == contratoLaboralId)
                    .Select(c => new ContratoLaboralreaderDto
                    {
                        ContratoLaboralId = c.ContratoLaboralId,
                        EmpleadoId = c.EmpleadoId,

                        NombreEmpleado = c.Empleado != null
                            ? $"{c.Empleado.PrimerNombre} {c.Empleado.PrimerApellido}".Replace("  ", " ").Trim()
                            : "Sin Asignar",
                        Documento = c.Empleado != null ? c.Empleado.Documento : string.Empty,

                        EmpresaId = c.EmpresaId,
                        NombreEmpresa = c.Empresa != null ? c.Empresa.NombreEmpresa : "Sin Asignar",
                        Nit = c.Empresa != null ? c.Empresa.Nit : null,

                        TipoContratoId = c.TipoContratoId,
                        NombreTipoContrato = c.TipoContrato != null ? c.TipoContrato.NombreContrato : "Sin Asignar",

                        ContratoLaboralDetalleId = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().ContratoLaboralDetalleId : 0,

                        FechaInicio = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().FechaInicio : null,

                        FechaFinalizacion = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().FechaFinalizacion : null,

                        FechaTerminacion = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().FechaTerminacion : null,

                        CargoId = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().CargoId : 0,

                        NombreCargo = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null &&
                                      c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Cargo != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Cargo.CargoNombre
                            : "Sin Asignar",

                        CentroCosto = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().CentroCosto : string.Empty,

                        Salario = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Salario : 0,

                        Observacion = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Observacion : null,

                        FechaCreacion = c.FechaCreacion,
                        Activo = c.Activo
                    })
                    .FirstOrDefaultAsync();

                return contrato;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener el contrato laboral por ID: {ex.Message}", ex);
            }
        }

        // Crea un nuevo contrato laboral y su respectivo detalle inicial para un empleado
        public async Task<bool> CreateContratoLaboralAsync(CreateContratoLaboralDto contratoDto)
        {
            if (contratoDto == null)
                return false;

            try
            {
                var contratoActivo = await _context.ContratoLaboral
                    .AnyAsync(x => x.EmpleadoId == contratoDto.EmpleadoId && x.Activo == true);

                if (contratoActivo)
                {
                    throw new ArgumentException("No se puede crear el contrato al empleado, ya que existe un contrato activo actualmente");
                }

                var cabeceraContrato = new ContratoLaboral
                {
                    EmpresaId = contratoDto.EmpresaId,
                    EmpleadoId = contratoDto.EmpleadoId,
                    TipoContratoId = contratoDto.TipoContratoId,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                await _context.ContratoLaboral.AddAsync(cabeceraContrato);
                await _context.SaveChangesAsync();

                var detalleContrato = new ContratoLaboralDetalle
                {
                    ContratoLaboralId = cabeceraContrato.ContratoLaboralId,
                    FechaInicio = contratoDto.FechaInicio ?? DateTime.Today,
                    FechaFinalizacion = contratoDto.FechaFinalizacion,
                    CargoId = contratoDto.CargoId,
                    CentroCosto = contratoDto.CentroCosto,
                    Salario = contratoDto.Salario ?? 0m,
                    Observacion = string.Empty
                };

                await _context.ContratoLaboralDetalle.AddAsync(detalleContrato);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Error de BD: {errorReal}", ex);
            }
        }

        // Obtiene la lista completa de contratos asociados a un empleado por su EmpleadoId (Para la tabla de Historial)
        public async Task<List<ContratoLaboralreaderDto>> GetContratosPorEmpleadoAsync(int empleadoId)
        {
            try
            {
                var contrato = await _context.ContratoLaboral.AsNoTracking()
                    .Where(c => c.EmpleadoId == empleadoId)
                    .Select(c => new ContratoLaboralreaderDto
                    {
                        ContratoLaboralId = c.ContratoLaboralId,
                        EmpleadoId = c.EmpleadoId,

                        NombreEmpleado = c.Empleado != null
                            ? $"{c.Empleado.PrimerNombre} {c.Empleado.PrimerApellido}".Replace("  ", " ").Trim()
                            : "Sin Asignar",
                        Documento = c.Empleado != null ? c.Empleado.Documento : string.Empty,

                        EmpresaId = c.EmpresaId,
                        NombreEmpresa = c.Empresa != null ? c.Empresa.NombreEmpresa : "Sin Asignar",
                        Nit = c.Empresa != null ? c.Empresa.Nit : null,

                        TipoContratoId = c.TipoContratoId,
                        NombreTipoContrato = c.TipoContrato != null ? c.TipoContrato.NombreContrato : "Sin Asignar",

                        ContratoLaboralDetalleId = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().ContratoLaboralDetalleId : 0,

                        FechaInicio = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().FechaInicio : null,

                        FechaFinalizacion = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().FechaFinalizacion : null,

                        FechaTerminacion = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().FechaTerminacion : null,

                        CargoId = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().CargoId : 0,

                        NombreCargo = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null &&
                                      c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Cargo != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Cargo.CargoNombre
                            : "Sin Asignar",

                        CentroCosto = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().CentroCosto : string.Empty,

                        Salario = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Salario : 0,

                        Observacion = c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault() != null
                            ? c.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault().Observacion : null,

                        FechaCreacion = c.FechaCreacion,
                        Activo = c.Activo
                    }).ToListAsync();

                return contrato;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener los contratos laborales del empleado: {ex.Message}", ex);
            }
        }

        public async Task<ContratoLaboralreaderDto> UpdateContratoAsync(UpdateContratoDto update)
        {
            try
            {
                var contratoDb = await _context.ContratoLaboral
                    .FirstOrDefaultAsync(x => x.ContratoLaboralId == update.ContratoLaboralId);

                if (contratoDb == null)
                {
                    throw new KeyNotFoundException("El contrato laboral no fue encontrado.");
                }

                if (contratoDb.Activo != true)
                {
                    throw new InvalidOperationException("No se pueden modificar contratos que se encuentran inactivos.");
                }

                contratoDb.EmpresaId = update.EmpresaId;
                contratoDb.TipoContratoId = update.TipoContratoId;

                var detalleDb = await _context.ContratoLaboralDetalle
                    .OrderByDescending(d => d.FechaInicio)
                    .FirstOrDefaultAsync(d => d.ContratoLaboralId == update.ContratoLaboralId);

                if (detalleDb != null)
                {
                    detalleDb.FechaInicio = update.FechaInicio ?? detalleDb.FechaInicio;
                    detalleDb.FechaFinalizacion = update.FechaFinalizacion;
                    detalleDb.CargoId = update.CargoId;
                    detalleDb.CentroCosto = update.CentroCosto;
                    detalleDb.Salario = update.Salario ?? detalleDb.Salario;

                    _context.ContratoLaboralDetalle.Update(detalleDb);
                }

                _context.ContratoLaboral.Update(contratoDb);
                await _context.SaveChangesAsync();

                var contratoDto = new ContratoLaboralreaderDto
                {
                    ContratoLaboralId = contratoDb.ContratoLaboralId,
                    EmpresaId = contratoDb.EmpresaId,
                    TipoContratoId = contratoDb.TipoContratoId,
                    CargoId = detalleDb?.CargoId ?? 0,
                    CentroCosto = detalleDb?.CentroCosto,
                    Salario = detalleDb?.Salario ?? 0,
                    FechaInicio = detalleDb?.FechaInicio ?? DateTime.MinValue,
                    FechaFinalizacion = detalleDb?.FechaFinalizacion,
                    Activo = contratoDb.Activo
                };

                return contratoDto;
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el contrato: {ex.Message}");
            }
        }

        // Inhabilita o termina un contrato registrando una observación y fecha de terminación
        public async Task<bool> InhabilitarContratoAsync(InhabilitarContratoDto dto)
        {
            try
            {
                var contratoLaboral = await _context.ContratoLaboral
                    .FirstOrDefaultAsync(c => c.ContratoLaboralId == dto.ContratoLaboralId);

                if (contratoLaboral == null)
                {
                    return false;
                }

                // Evaluación segura de bool?: verifica si es false o null
                if (contratoLaboral.Activo == false)
                {
                    throw new InvalidOperationException("No se puede inactivar el contrato laboral porque ya se encuentra inactivo.");
                }

                var contratoDetalle = await _context.ContratoLaboralDetalle
                    .FirstOrDefaultAsync(d => d.ContratoLaboralId == dto.ContratoLaboralId);

                if (contratoDetalle == null)
                {
                    return false;
                }

                contratoLaboral.Activo = dto.Activo;
                contratoDetalle.Observacion = dto.Observacion;
                contratoDetalle.FechaTerminacion = dto.FechaTerminacion;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (InvalidOperationException)
            {
                // Re-lanza la excepción de negocio para ser capturada por el controlador
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al terminar contrato: {ex.Message}", ex);
            }
        }
    }
    }
