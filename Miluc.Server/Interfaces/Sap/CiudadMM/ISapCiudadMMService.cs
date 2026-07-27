using Miluc.Server.Models.Sap;

namespace Miluc.Server.Interfaces.Sap.CiudadMM
{
    public interface ISapCiudadMMService
    {
        public Task<(List<BPCO_MU> Data, int TotalRegistros)>GetAllBPCO_MUAsync(string? buscar = null, int pagina = 1, int? cantidad = null);
    }
}
