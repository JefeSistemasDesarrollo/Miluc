using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class EmpresaService(NominaDbContext _context) : IEmpresaService
    {
        public async Task<List<EmpresaReaderDto>> GetEmpresasAsync()
        {
            try { 
                var empresas =  await _context.Empresa.AsTracking()
                    .Select(e => new EmpresaReaderDto {


                        EmpresaId = e.EmpresaId,
                        Nit = e.Nit,
                        NombreEmpresa = e.NombreEmpresa,
                        FechaCreacion = e.FechaCreacion,
                        FechaActualizacion = e.FechaActualizacion,// DB puede ser NULL
                        Activo = e.Activo
                    }).ToListAsync();
                return empresas;

            }
            
            catch(Exception ex) 
            { 
                throw new Exception($"Error al obtener las empresas: {ex.Message}");

            }
        }
    }
}
