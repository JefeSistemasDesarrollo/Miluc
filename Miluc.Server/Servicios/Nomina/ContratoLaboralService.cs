using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class ContratoLaboralService(NominaDbContext _context) : IContratoLaboralService
    {
        public async Task<List<ContratoLabralreaderDto>> GetAllContratoLaboralAsync()
        {
            try
            {
                var contratoLab = await _context.ContratoLaboral.AsNoTracking()
                .Include(t => t.TipoContrato)
                .Include(e => e.Empresa)
                .Include(e => e.Empleado) 
                .Include(c => c.ContratoLaboralDetalle)
                .Select(c => new ContratoLabralreaderDto
                {


                    ContratoLaboralId = c.ContratoLaboralId,
                    EmpresaId = c.EmpresaId,
                    NombreTipoContrato=c.TipoContrato.NombreContrato,
                    //ContratoLaboralDetalleId = c.ContratoLaboralDetalleId,
                    FechaCreacion = c.FechaCreacion,
                     //RELACION CON EL NOMBRE DE LA EMPRESA
                    NombreEmpresa = c.Empresa.NombreEmpresa,
                    //RELACION CON EL EMPLEADO
                    EmpleadoId = c.EmpleadoId,
                    //  Relacion Concatenar  los nombres del empleado para mostrar el nombre completo
                    NombreEmpleado = $"{c.Empleado.PrimerNombre} {c.Empleado.SegundoNombre} {c.Empleado.PrimerApellido} {c.Empleado.SegundoApellido}",
                    //Relacion con el nombre del tipo de contrato
                    TipoContratoId = c.TipoContratoId,

                }).ToListAsync();
                return contratoLab;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los contratos laborales: {ex.Message}");
            }
        }

        public async Task<(List<ContratoLabralreaderDto> data, int CantidadDeRegistros)> GetAllContratoLaboralAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 200;
                var  query = _context.ContratoLaboral.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(x => x.Empleado.PrimerNombre.Contains(filtro) || x.Empleado.PrimerApellido.Contains(filtro));
                }
                var totalRegistros = query.Count();
                var  contrato = await query
                .Include(t => t.TipoContrato)
                .Include(e => e.Empresa)
                //.Include(e => e.Empleado)
                .Include(c => c.ContratoLaboralDetalle)
                .Select(c => new ContratoLabralreaderDto
                {


                    ContratoLaboralId = c.ContratoLaboralId,
                    EmpresaId = c.EmpresaId,
                    NombreTipoContrato = c.TipoContrato.NombreContrato,
                   
                    FechaCreacion = c.FechaCreacion,
                    //RELACION CON EL NOMBRE DE LA EMPRESA
                    NombreEmpresa = c.Empresa.NombreEmpresa,
                    //RELACION CON EL EMPLEADO
                    EmpleadoId = c.EmpleadoId,
                    //  Relacion Concatenar  los nombres del empleado para mostrar el nombre completo
                    NombreEmpleado = $"{c.Empleado.PrimerNombre} {c.Empleado.SegundoNombre} {c.Empleado.PrimerApellido} {c.Empleado.SegundoApellido}",
                    //Relacion con el nombre del tipo de contrato
                    TipoContratoId = c.TipoContratoId,

                }).Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .ToListAsync();
                return (contrato, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los contratos laborales: {ex.Message}");
            }
        }

        public async Task<ContratoLabralreaderDto> GetContratoById(int idcontrato)
        {
            try
            {
                var contratoLabId = await _context.ContratoLaboral.AsNoTracking()
                .Include(t => t.TipoContrato)
                .Include(e => e.Empresa)
                .Include(e => e.Empleado)
                .Include(c => c.ContratoLaboralDetalle)
                .Where(c => c.ContratoLaboralId == idcontrato)
                .Select(c => new ContratoLabralreaderDto
                {


                    ContratoLaboralId = c.ContratoLaboralId,
                    EmpresaId = c.EmpresaId,
                    NombreTipoContrato = c.TipoContrato.NombreContrato,
                    //ContratoLaboralDetalleId = c.ContratoLaboralDetalleId,
                    FechaCreacion = c.FechaCreacion,
                    //RELACION CON EL NOMBRE DE LA EMPRESA
                    NombreEmpresa = c.Empresa.NombreEmpresa,
                    //RELACION CON EL EMPLEADO
                    EmpleadoId = c.EmpleadoId,
                    //  Relacion Concatenar  los nombres del empleado para mostrar el nombre completo
                    NombreEmpleado = $"{c.Empleado.PrimerNombre} {c.Empleado.SegundoNombre} {c.Empleado.PrimerApellido} {c.Empleado.SegundoApellido}",
                    //Relacion con el nombre del tipo de contrato
                    TipoContratoId = c.TipoContratoId,
                    detalleContratoDto = c.ContratoLaboralDetalle.Select(c => new DetalleContratoLaboralDto
                    {
                        ContratoLaboralDetalleId = c.ContratoLaboralDetalleId,
                        FechaInicio = c.FechaInicio,
                        FechaFinalizacion = c.FechaFinalizacion,
                        Cargo = c.Cargo,
                        CentroCosto = c.CentroCosto,
                        Salario = c.Salario,
                    }).ToList()


                }).FirstOrDefaultAsync();

                if (contratoLabId == null)
                    throw new Exception($"error al buscar contrato labolar{idcontrato} ");


                return contratoLabId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los contratos laborales: {ex.Message}");
            }
        }
    }
    }
