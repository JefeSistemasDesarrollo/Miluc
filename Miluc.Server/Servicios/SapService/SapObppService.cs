using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Obpp;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Rutas;

namespace Miluc.Server.Servicios.SapService
{
    public class SapObppService (SapDbContex sapDbContex): ISapObppService
    {
        public async Task<List<SapObppDto>> GetObppsAsync()
        {
            try
            {

                var listar = await sapDbContex.OBPP.AsNoTracking()
                    .Select(r=>new SapObppDto
                    {
                        PrioCode=r.PrioCode,
                        PrioDesc=r.PrioDesc,
                    }).
                    ToListAsync();

                


                return listar;
            }
            catch (Exception ex)
            {

                throw new Exception("RUTAS");
            }
        }
    }
}
