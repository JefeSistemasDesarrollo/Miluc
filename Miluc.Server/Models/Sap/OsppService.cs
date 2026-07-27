using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Ospp;

namespace Miluc.Server.Models.Sap
{
    public class OsppService(SapDbContex sapDbContex) : ISapOsppService
    {
        

        public async Task<List<OSPPrecioEspecialSap>> GetPreciosEspecialesAsync()
        {
            try
            {
                var preciosEspeciales = await sapDbContex.OSPP.AsNoTracking().ToListAsync();

                return preciosEspeciales;
            }
            catch (Exception ex) 
            { 
                throw new Exception($"Error al obtener precios especiales: {ex.Message}", ex );

            }
        }   
    }
}
