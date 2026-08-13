namespace Miluc.Shared.DTOs.Sap.Pedidos
{
    public class PedidoCrearDetalleLineaDTO
    {
        public string ItemCode { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal  U_CantidadKg { get; set; }

        public decimal U_UnidadSal {  get; set; }
    }
}
