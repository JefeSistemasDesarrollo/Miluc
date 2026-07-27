using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Oslp;
using Miluc.Shared.DTOs.Sap.Vendedor;

namespace Miluc.Server.Servicios.SapService
{
    public class SapOslpService(SapDbContex _dbContex) : ISapOslpService
    {
        public async Task<List<SapVendedorReaderDto>> GetListOslpAsync()
        {
            try
            {

                var listOslp = await _dbContex.OSLP.AsNoTracking().
                    
                    Where(x => x.Active == 'Y').
                    Select(o => new SapVendedorReaderDto
                    {
                        SlpCode = o.SlpCode,
                        SlpName = o.SlpName
                    }).ToListAsync();

                return listOslp;

            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener la lista de OSLP: " + ex.Message);

            }
        }
    }
}
