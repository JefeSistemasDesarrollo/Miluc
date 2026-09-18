using System.Text.Json.Serialization;

namespace Miluc.Server.Models.Sap
{
    public class RDN1
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string? ItemCode { get; set; }
        public string? Dscription { get; set; }
        public string? BaseRef { get; set; }
        public int? BaseEntry { get; set; }
        public int? BaseLine { get; set; }
        public char? LineStatus { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? LineTotal { get; set; }
        public string? UnitMsr { get; set; }
        public string? WhsCode { get; set; }
        public DateTime? Docdate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public decimal? Width1 { get; set; }
        public decimal? U_CantidadKg { get; set; }  //cantidad de kg por unidad de medida de compra
        public decimal? U_UnidadSal { get; set; } //equivalentes de unidades de medida de venta por unidad de medida de compra
        public decimal? VatSum { get; set; }
        public int? U_Picking { get; set; }
        public ORDN? ORDN { get; set; }
        //[JsonIgnore]
        public DLN1? DLN1 { get; set; }
        //public ICollection<DLN1>   DLN1 { get; set; } = new List<DLN1>();
    }
}
