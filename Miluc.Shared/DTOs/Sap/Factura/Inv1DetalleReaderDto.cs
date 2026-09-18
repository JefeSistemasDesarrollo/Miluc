namespace Miluc.Shared.DTOs.Sap.Factura
{
    public class Inv1DetalleReaderDto
    {
        public int DocEntry { get; set; }
        public int? LineNum { get; set; }
        public string? BaseRef { get; set; }
        public string? BaseType { get; set; }
        public int? BaseEntry { get; set; }
        public int? BaseLine { get; set; }
        public string? ItemCode { get; set; }
        public string? Dscription { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public decimal? Rate { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal? U_CantidadKg { get; set; }
        public decimal? U_UnidadSal { get; set; }

        public string? WhsCode { get; set; }
        public int? U_Picking { get; set; }
        public decimal? Width1 { get; set; }
        public string? unitMsr { get; set; }

        public OinvReaderDto? OinvReader { get; set; }
    }
}
