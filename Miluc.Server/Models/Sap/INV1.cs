using System.Text.Json.Serialization;

namespace Miluc.Server.Models.Sap
{
    public class INV1
    {
        public int DocEntry { get; set; }
        public int ? LineNum { get; set; }
        public string ?BaseRef { get; set; }
        public string ? BaseType { get; set; }
        public int ? BaseEntry { get; set; }
        public int ? BaseLine { get; set; }
        public int ? U_Picking { get; set; }
        public string ? ItemCode { get; set; }
        public string ? Dscription { get; set; }
        public decimal ? Quantity { get; set; }
        public decimal ? Price { get; set; }
        public string ? Currency { get; set; }
        public decimal ? Rate { get; set; }
        public decimal ? LineTotal { get; set; }
        public decimal ? VatSum { get; set; }
        public string ? WhsCode { get; set; }
        public decimal ? Width1 { get; set; }
        public string? unitMsr { get; set; }

        public decimal? U_CantidadKg { get; set; }
        public decimal? U_UnidadSal { get; set; }
        public OINV ? OINV { get; set; }
        
        
        //[JsonIgnore]
       public RDR1 ?RDR1 { get; set; }

      

        public DLN1? DLN1 { get; set; }
        public ICollection<RIN1> RIN1 { get; set; }=new List<RIN1>();

      


        //[JsonIgnore]
        //public ICollection<DLN1> DLN1 { get; set; } = new List<DLN1>();

    }
}
