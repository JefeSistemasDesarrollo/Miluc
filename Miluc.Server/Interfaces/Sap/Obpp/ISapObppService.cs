using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Rutas;

namespace Miluc.Server.Interfaces.Sap.Obpp
{
    public interface ISapObppService
    {
        public Task<List<SapObppDto>>GetObppsAsync();
    }
}
