using Miluc.Server.Models.Sap;

namespace Miluc.Server.Interfaces.Sap.Ospp
{
    public interface ISapOsppService
    {
        Task<List<OSPPrecioEspecialSap>> GetPreciosEspecialesAsync();
    }
}
