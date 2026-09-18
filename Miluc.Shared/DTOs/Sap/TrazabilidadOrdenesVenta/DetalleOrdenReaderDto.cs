using Miluc.Shared.DTOs.Sap.NotaDeEntrega;

namespace Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta
{
    public class DetalleOrdenReaderDto
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string Dscription { get; set; } = string.Empty;
        public decimal? Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal? Price { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal? VatSum { get; set; }
        public decimal? Weight1 { get; set; }
        public decimal? VatPrcnt { get; set; }
        public string? TaxCode { get; set; } = string.Empty;
        public decimal? U_CantidadKg { get; set; }  //cantidad de kg por unidad de medida de compra
        public decimal? U_UnidadSal { get; set; } //equivalentes de unidades de medida de venta por unidad de medida de compra
        // public decimal  BaseSum { get; set; } //precio base del articulo en la lista de precios}
        public string? UnitMsr { get; set; } = string.Empty;
        public string? WhsCode { get; set; } = string.Empty;

        public ICollection<Dln1DetalleReaderDto> detalleNotaEntrega { get; set; } = new List<Dln1DetalleReaderDto>();



    }
}
