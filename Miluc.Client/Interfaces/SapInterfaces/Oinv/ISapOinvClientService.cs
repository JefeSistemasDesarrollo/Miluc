using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.Oinv
{
    public interface ISapOinvClientService
    {
        Task<ResponseAPI<int>> ObtenerPedidoDesdeFacturaObtenerPedidoDesdeFactura(int docEntry);
    }
}
