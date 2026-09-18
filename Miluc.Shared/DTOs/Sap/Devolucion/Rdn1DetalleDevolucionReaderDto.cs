namespace Miluc.Shared.DTOs.Sap.Devolucion
{
    public class Rdn1DetalleDevolucionReaderDto
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

        public int ? BaseEntry { get; set; }
        public int ? U_Picking { get; set; }

        //public int DocEntry { get; set; }
        //public int LineNum { get; set; }
        //public string? ItemCode { get; set; }
        //public string? Dscription { get; set; }
        //public string? BaseRef { get; set; }
        //public int? BaseEntry { get; set; }
        //public int? BaseLine { get; set; }
        //public char? LineStatus { get; set; }
        //public decimal? Quantity { get; set; }
        //public decimal? Price { get; set; }
        //public decimal? LineTotal { get; set; }
        //public string? UnitMsr { get; set; }
        //public string? WhsCode { get; set; }
        //public DateTime? Docdate { get; set; }
        //public DateTime? DocdueDate { get; set; }
        //public string? Width1 { get; set; }
        //public decimal? VatSum { get; set; }

        public OrdnReaderDto OrdnReader { get; set; } = new OrdnReaderDto();
    }
}
