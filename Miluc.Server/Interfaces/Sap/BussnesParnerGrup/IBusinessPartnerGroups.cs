using Miluc.Shared.DTOs.Sap.GrupoDeVenta;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Sap.BussnesParnerGrup
{
    public interface IBusinessPartnerGroups
    {
        public Task<ResponseAPI<List<BusinessPartnerGroupsDto>>> GetallBusinessPartnerGroups();
    }
}
