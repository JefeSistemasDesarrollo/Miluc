namespace Miluc.Shared.DTOs.Sap.InformesComercial
{
    public class VentasPorVendedorDto
    {
        public int CodigoVendedor { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public string CodigoCliente { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public int Documento { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal TotalVenta { get; set; }
    }
}
