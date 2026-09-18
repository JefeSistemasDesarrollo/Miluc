using Newtonsoft.Json;

namespace Miluc.Server.Models.Sap
{
    public class DLN1
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public int ?BaseEntry { get; set; }
        public int ?BaseLine { get; set; }
        public int ? BaseType { get; set; }
        public string? BaseRef { get; set; }
        public int? BaseDocNum { get; set; }
        public char? LineStatus { get; set; }
        public string? ItemCode { get; set; }
        public string? Dscription { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Rate { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal? U_UnidadSal { get; set; }
        public decimal? U_CantidadKg { get; set; }
        //public decimal? PriceTotal { get; set; }

        public string? WhsCode { get; set; }
        public DateTime? DocDate { get; set; }
        public char? BasePrice { get; set; }
        public string? BaseCard { get; set; }
        public decimal? TotalSumSy { get; set; }
        public decimal? PriceAfVAT { get; set; }
        public string? ShipToDesc { get; set; }
        public string? unitMsr { get; set; }
        public int ?U_Picking { get; set; }
        //public decimal? U_CantidadKg { get; set; }
        //public decimal? U_UnidadSal { get; set; }

        //[JsonIgnore]
        public ODLN ODLN { get; set; }

        [JsonIgnore]
        public RDR1? RDR1 { get; set; }

        [JsonIgnore]
        public ICollection<INV1> INV1 { get; set; }

        //[JsonIgnore]
        public ICollection<RDN1> RDN1 { get; set; } = new List<RDN1>();
    }
}
