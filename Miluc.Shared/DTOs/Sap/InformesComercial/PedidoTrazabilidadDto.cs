namespace Miluc.Shared.DTOs.Sap.InformesComercial
{
    public class PedidoTrazabilidadDto
    {
        public string? CodCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? Ruta { get; set; }
        public string? Sucursal { get; set; }
        public string? OrigenPedido { get; set; }
        public string? Producto { get; set; }
        public string? Descripcion { get; set; }
        public string? UM { get; set; }
        public DateTime? FechaPedido { get; set; }
        public DateTime? FechaComprometido { get; set; }
        public int? Pedido { get; set; }
        public decimal? Cantidad { get; set; }
        public DateTime? FechaNotaEntrega { get; set; }
        public int? NotaEntrega { get; set; }
        public decimal? CantidadNotaEntrega { get; set; }
        public DateTime? FechaFactura { get; set; }
        public int? Factura { get; set; }
        public decimal? CantidadFactura { get; set; }
        public decimal? TotalFactura { get; set; }
        public DateTime? FechaNotaCredito { get; set; }
        public int? NotaCredito { get; set; }
        public decimal? CantidadNotaCredito { get; set; }
        public decimal? ValorNotaCredito { get; set; }
        public decimal? ValorNetoFactura { get; set; }
        public string? Vendedor { get; set; }
    }
}
