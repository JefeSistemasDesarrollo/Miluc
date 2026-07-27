using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.MedioTransporteDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class MedioTransporteService(NominaDbContext _Context) : IMedioTransporteService
    {
        public async Task<List<MedioTransporteReaderDto>> GetMedioTransporteAsync()
        {
            try
            {
                var transporte = await _Context.MedioTransportes.AsNoTracking()
                    .Select(m => new MedioTransporteReaderDto
                    {
                        MedioTransporteId = m.MedioTransporteId,
                        Nombre = m.Nombre,
                    }).ToListAsync();
                return transporte;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al encontrar MedioTransporte", ex);

            }
        }
    }
}