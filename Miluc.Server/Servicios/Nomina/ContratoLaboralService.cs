using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;

using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class ContratoLaboralService(NominaDbContext _context) : IContratoLaboralService
    {
        public Task<ContratoLabralreaderDto> GetContratoById(int idcontrato)
        {
            throw new NotImplementedException();
        }




        //public async Task<ContratoLabralreaderDto> GetContratoById(int idcontrato)
        //{
        //    try
        //    {
        //        var contratoLabId = await _context.ContratoLaboral.AsNoTracking()
        //        .Include(t => t.TipoContrato)
        //        .Include(e => e.Empresa)
        //        .Include(e => e.Empleado)
        //        .Include(c => c.ContratoLaboralDetalle)
        //        .Where(c => c.ContratoLaboralId == idcontrato)
        //        .Select(c => new ContratoLabralreaderDto
        //        {


        //            ContratoLaboralId = c.ContratoLaboralId,
        //            EmpresaId = c.EmpresaId,
        //            NombreTipoContrato = c.TipoContrato.NombreContrato,
        //            //ContratoLaboralDetalleId = c.ContratoLaboralDetalleId,
        //            FechaCreacion = c.FechaCreacion,
        //            //RELACION CON EL NOMBRE DE LA EMPRESA
        //            NombreEmpresa = c.Empresa.NombreEmpresa,
        //            //RELACION CON EL EMPLEADO
        //            EmpleadoId = c.EmpleadoId,
        //            //  Relacion Concatenar  los nombres del empleado para mostrar el nombre completo
        //            NombreEmpleado = $"{c.Empleado.PrimerNombre} {c.Empleado.SegundoNombre} {c.Empleado.PrimerApellido} {c.Empleado.SegundoApellido}",
        //            //Relacion con el nombre del tipo de contrato
        //            TipoContratoId = c.TipoContratoId,
        //            //ContratoLaboralDetalleId = c.ContratoLaboralDetalle.Select(c => new DetalleContratoLaboralDto
        //            // {
        //            //    ContratoLaboralDetalleId = c.ContratoLaboralDetalleId,
        //            //     FechaInicio = c.FechaInicio,
        //            //     FechaFinalizacion = c.FechaFinalizacion,
        //            //     Cargo = c.Cargo,
        //            //     CentroCosto = c.CentroCosto,
        //            //     Salario = c.Salario,
        //            // }).ToList()


        //        }).FirstOrDefaultAsync();

        //        if (contratoLabId == null)
        //            throw new Exception($"error al buscar contrato labolar{idcontrato} ");


        //        return contratoLabId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al obtener los contratos laborales: {ex.Message}");
        //    }
        //}


        public async Task<(List<ContratoLabralreaderDto> data, int CantidadDeRegistros)> GetContratoLaboralAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadtop = cantidad ?? 20;

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
                    .Skip((page - 1) * cantidadtop)
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
        //public async Task<bool> UpsertContratoLaboralAsync(UpdateContratoLaboralDto dto)
        //{
        //    try
        //    {
        //        // 1. Buscar si ya existe el contrato cargando sus detalles asociados
        //        var contratoExistente = await _context.ContratoLaboral
        //            .Include(c => c.ContratoLaboralDetalle)
        //            .FirstOrDefaultAsync(c => (dto.ContratoLaboralId > 0 && c.ContratoLaboralId == dto.ContratoLaboralId)
        //                                   || (c.EmpleadoId == dto.EmpleadoId));

        //        if (contratoExistente == null)
        //        {

        //            var nuevoContrato = new ContratoLaboral
        //            {
        //                EmpleadoId = dto.EmpleadoId,
        //                EmpresaId = dto.EmpresaId,
        //                TipoContratoId = dto.TipoContratoId,
        //                FechaCreacion = DateTime.Now,
        //                ContratoLaboralDetalle = new List<ContratoLaboralDetalle>
        //        {
        //            new ContratoLaboralDetalle
        //            {
        //                FechaInicio = dto.FechaInicio,
        //                FechaFinalizacion = dto.FechaFinalizacion == default(DateTime) ? null : dto.FechaFinalizacion,
        //                Cargo = dto.Cargo,
        //                CentroCosto = dto.CentroCosto,
        //                Salario = dto.Salario
        //            }
        //        }
        //            };

        //            _context.ContratoLaboral.Add(nuevoContrato);
        //        }
        //        else
        //        {
        //            // ACTUALIZAR CONTRATO EXISTENTE

        //            contratoExistente.EmpresaId = dto.EmpresaId;
        //            contratoExistente.TipoContratoId = dto.TipoContratoId;

        //            // Buscamos el detalle a actualizar (por Id o tomamos el más reciente)
        //            var detalleExistente = contratoExistente.ContratoLaboralDetalle
        //                .FirstOrDefault(d => dto.ContratoLaboralDetalleId > 0 && d.ContratoLaboralDetalleId == dto.ContratoLaboralDetalleId)
        //                ?? contratoExistente.ContratoLaboralDetalle.OrderByDescending(d => d.FechaInicio).FirstOrDefault();

        //            if (detalleExistente != null)
        //            {
        //                // Actualizamos los valores del detalle
        //                detalleExistente.FechaInicio = dto.FechaInicio;
        //                detalleExistente.FechaFinalizacion = dto.FechaFinalizacion == default(DateTime) ? null : dto.FechaFinalizacion;
        //                detalleExistente.Cargo = dto.Cargo;
        //                detalleExistente.CentroCosto = dto.CentroCosto;
        //                detalleExistente.Salario = dto.Salario;
        //            }
        //            else
        //            {
        //                // Si por alguna razón el contrato no tenía detalle en BD, lo agregamos
        //                contratoExistente.ContratoLaboralDetalle.Add(new ContratoLaboralDetalle
        //                {
        //                    FechaInicio = dto.FechaInicio,
        //                    FechaFinalizacion = dto.FechaFinalizacion == default(DateTime) ? null : dto.FechaFinalizacion,
        //                    Cargo = dto.Cargo,
        //                    CentroCosto = dto.CentroCosto,
        //                    Salario = dto.Salario
        //                });
        //            }
        //    }

        //    // Confirmar los cambios
        //    return await _context.SaveChangesAsync() > 0;
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception($"Error al procesar el contrato laboral: {ex.Message}", ex);
        //}
    }





    }
 
    
