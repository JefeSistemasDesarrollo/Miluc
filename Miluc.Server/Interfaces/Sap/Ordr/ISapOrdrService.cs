using Miluc.Shared.DTOs.Sap.Pedidos;

namespace Miluc.Server.Interfaces.Sap.Ordr
{
    public interface ISapOrdrService
    {
        Task<OrdersReaderDto> CreatePedidoAsyc(PedidoCreateDto pedidoCreateDto);

        Task<OrdersReaderDto> GetOrderByIdAsync(int id);

        Task<(List<OrdersReaderDto> data, int TotalRegistros)> ListarPedidosAsync(string? buscar = null, int? pagina = null, int? cantidad = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);

        Task<OrdersReaderDto> UpdatePedidoAsync(PedidoUpdateDto pedidoUpdateDto);
        // Task<OrdersReaderDto> DeletePedidoByIdAsync(PedidoUpdateDto pedidoUpdateDto);
        Task<bool> CancelPedidoAsync(int docEntry);
       // Task DeletePedidoByIdAsync(int id);


    }
}
