using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Opln;
using Miluc.Shared.DTOs.Sap.Opln;

namespace Miluc.Server.Servicios.SapService
{
    public class SapOplnService(SapDbContex _dbContex) : ISapOplnService
    {
        public async Task<List<OplnReaderDto>> GetListOplnAsync()
        {
            try
            {
                var listOpln = await _dbContex.OPLN.AsNoTracking()
                    .Select(o=>new OplnReaderDto
                    {
                        ListNum=o.ListNum,
                        ListName=o.ListName,

                    })
                    .ToListAsync();


                


                return listOpln;

            }
            catch (Exception ex)
            {


                throw new Exception("" + ex.Message);

            }
        }
    }
}
