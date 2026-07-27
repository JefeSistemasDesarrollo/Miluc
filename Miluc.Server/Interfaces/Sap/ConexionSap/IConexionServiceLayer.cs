using Miluc.Shared.DTOs.Sap.ConexionSapServiceLayer;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Sap.ConexionSap
{
    public interface IConexionServiceLayer
    {
        Task<ResponseAPI<ConexionSapServiceLayerDto>> ConexionSapService();

        Task<ResponseAPI<bool>> LogoutAsync(string urlServiceLayer, string sessionId, string routeId);
    }
}
