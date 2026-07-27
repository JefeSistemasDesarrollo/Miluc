using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Itm1Sap;
using Miluc.Server.Models.Sap;

namespace Miluc.Server.Servicios.SapService
{
    public class SapItm1Service(SapDbContex dbContex) : ISapItm1Service
    {
        public async Task<List<ITM1>> GetListItm1Async()
        {
            try
            {
                var result = await dbContex.ITM1.ToListAsync();

                return result;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de ITM1: {ex.Message}", ex);
            }
        }
    }
}
