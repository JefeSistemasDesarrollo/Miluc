using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class ContratoLaboralDetalleService(NominaDbContext _context) : IContratoLaboralDetalleService
    {
        public async Task<List<ContratoLaboralDetalleDto>> GetContratoLaboralDetallesAsync()
        {
            try
            {
                var detalles =  await _context.ContratoLaboralDetalle.AsNoTracking()
                    .Select(d => new ContratoLaboralDetalleDto
                {
                    ContratoLaboralDetalleId = d.ContratoLaboralDetalleId,
                    ContratoLaboralId = d.ContratoLaboralId,
                    FechaInicio = DateTime.Now,
                    FechaFinalizacion = DateTime.Now,
                    CargoId = d.CargoId,
                    Observacion = d.Observacion,
                    CentroCosto = d.CentroCosto,
                    Salario = d.Salario
                }).ToListAsync();
                return detalles;
            }
            catch (Exception ex)
            
            {
                throw new Exception("Error al obtener los detalles de contratos laborales", ex);
            }
        }
    }
}
