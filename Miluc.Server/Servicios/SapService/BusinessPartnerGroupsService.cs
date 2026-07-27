using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.BussnesParnerGrup;
using Miluc.Shared.DTOs.Sap.GrupoDeVenta;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Servicios.SapService
{
    public class BusinessPartnerGroupsService(SapDbContex sapdbContex) : IBusinessPartnerGroups
    {
        public async Task<ResponseAPI<List<BusinessPartnerGroupsDto>>> GetallBusinessPartnerGroups()
        {
            try
            {
                var result = await sapdbContex.OCRG.AsNoTracking()
                    .Select(x => new BusinessPartnerGroupsDto
                    {
                        GroupCode = x.GroupCode,
                        GroupName = x.GroupName
                    })
                    .ToListAsync();



                return new ResponseAPI<List<BusinessPartnerGroupsDto>>().SuccessResponse(true, "Grupos de socios comerciales obtenidos correctamente", result, result.Count);




            }
            catch (Exception ex) 
            {
                throw new Exception("Ocurrió un error al obtener los grupos de socios comerciales: {ex.Message}");

            }
        }
    }
}
