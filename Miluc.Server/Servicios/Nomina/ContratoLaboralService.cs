using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;

using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class ContratoLaboralService(NominaDbContext _context) : IContratoLaboralService
    {
        public Task<ContratoLabralreaderDto> GetContratoById(int idcontrato)
        {
            throw new NotImplementedException();
        }
        public async Task<(List<ContratoLabralreaderDto> data, int CantidadDeRegistros)> GetContratoLaboralAsync(string? filtro = null, int ? page =null, int? cantidad = null)
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
                        EmpleadoId = x.EmpleadoId,
                        PrimerNombre = x.PrimerNombre,
                        SegundoNombre = x.SegundoNombre,
                        PrimerApellido = x.PrimerApellido,
                        SegundoApellido = x.SegundoApellido,
                        Documento = x.Documento,

                        Contrato = x.ContratoLaboral
                            .OrderByDescending(c => c.FechaCreacion)
                            .Select(c => new
                            {
                                c.ContratoLaboralId,
                                c.FechaCreacion,
                                c.Activo, // Lee el bit directamente de la tabla ContratoLaboral
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
                                        d.Cargo,
                                        d.CentroCosto,
                                        d.Salario
                                    })
                                    .FirstOrDefault()
                            })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                var contratos = datosIntermedios.Select(x => new ContratoLabralreaderDto
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

                    Cargo = x.Contrato?.Detalle?.Cargo ?? "Sin Asignar",
                    CentroCosto = x.Contrato?.Detalle?.CentroCosto ?? "Sin Asignar",
                    Salario = x.Contrato?.Detalle?.Salario ?? 0,

                    Activo = x.Contrato?.Activo ?? false
                }).ToList();

                return (contratos, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener contratos laborales: {ex.Message}", ex);
            }
        }

        public Task<bool> UpsertContratoLaboralAsync(UpdateContratoLaboralDto dto)
        {
            throw new NotImplementedException();
        }

    }

}



