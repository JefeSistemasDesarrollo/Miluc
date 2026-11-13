namespace Miluc.Shared.DTOs.Sap.InformesComercial
{
    public  class ConsolidadoCanalDistribucion
    {
        public string? Vendedor { get; set; }
        public string? Articulos { get; set; }
        public string? NombreArticulo { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Peso { get; set; }
        public decimal? PesoKilogramos { get; set; }
        public decimal? LineTotalFactura { get; set; }
        //public string? UnidadDeMedidaFactura { get; set; }
        //public decimal? ValorTotalFactura { get; set; }
        public string? GrupoInventario { get; set; }
        public string? Canal { get; set; }
    }
}
