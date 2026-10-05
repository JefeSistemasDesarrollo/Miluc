namespace Miluc.Shared.DTOs.Sap.InformesComercial
{
    public class CanalDeDistribucionReaderDto
    {
        public string? CodigoCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? Sucursal { get; set; }
        public string? Canal { get; set; }

        public int? DocEntry { get; set; }
        public int? Documento { get; set; }
        public DateTime? FechaContabilizacion { get; set; }
        public DateTime? FechaEntrega { get; set; }

        //
        public string? Articulos { get; set; }
        public string? NombreArticulo { get; set; }
        public string? GrupoInventario { get; set; }
        public string? Vendedor { get; set; }
        public string? Ruta { get; set; }
        public string? VendedorAsignado { get; set; }
        public string? VendedorFactura { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Peso { get; set; }
        public decimal? LineTotal { get; set; }
        public string? UnidadDeMedida { get; set; }
        public decimal? ValorTotal { get; set; }
        public string? Correo { get; set; }
    }
}
