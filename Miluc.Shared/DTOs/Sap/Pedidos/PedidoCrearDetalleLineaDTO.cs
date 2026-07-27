namespace Miluc.Shared.DTOs.Sap.Pedidos
{
    public class PedidoCrearDetalleLineaDTO
    {
        public string ItemCode { get; set; } = string.Empty;

        public decimal Quantity { get; set; }
    }
}
