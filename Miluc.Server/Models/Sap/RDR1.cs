using System.Text.Json.Serialization;

namespace Miluc.Server.Models.Sap
{
    public class RDR1
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; } 
        public string Dscription { get; set; }
        public decimal? Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal? Price { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal? VatSum { get; set; }
        public decimal? Weight1 { get; set; }
        public decimal? VatPrcnt { get; set; }
        public string? TaxCode { get; set; }
        public decimal? U_CantidadKg { get; set; }  //cantidad de kg por unidad de medida de compra
        public decimal? U_UnidadSal { get; set; } //equivalentes de unidades de medida de venta por unidad de medida de compra
        public int? BaseType { get; set; }
        // public decimal  BaseSum { get; set; } //precio base del articulo en la lista de precios}
        public string ? UnitMsr { get; set; } 
        public string? WhsCode { get; set; }
        public int? BaseEntry { get; set; }
        public int? U_Picking { get; set; }

        public ORDR? ORDR { get; set; }


        [JsonIgnore]
        public OITM OITM { get; set; }
        //public int U_Picking { get; set; }
        // public OK1_PICK_TPLACAS? piking { get; set; }

        [JsonIgnore]
        public ICollection<DLN1> DLN1 { get; set; }

        [JsonIgnore]
        public ICollection<INV1> INV1 { get; set; }= new List<INV1>();



        
    }
}
