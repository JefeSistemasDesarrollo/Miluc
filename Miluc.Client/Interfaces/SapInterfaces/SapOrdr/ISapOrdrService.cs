using Miluc.Shared.DTOs.Sap.Pedidos;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.SapOrdr
{
    public interface ISapOrdrService
    {
        Task<ResponseAPI<OrdersReaderDto>> CreatePedidoAsyc(PedidoCreateDto pedidoCreateDto);
        Task<ResponseAPI<List<OrdersReaderDto>>> ListarPedidosAsync(string? buscar = null, int? pagina = null, int? cantidad = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<ResponseAPI<OrdersReaderDto>> GetOrderByIdAsync(int DocEntry);

        Task<ResponseAPI<OrdersReaderDto>> UpdatePedidoAsync(PedidoUpdateDto pedidoUpdateDto);

        Task<ResponseAPI<bool>> CancelPedidoAsync(int docEntry);



    }
}
