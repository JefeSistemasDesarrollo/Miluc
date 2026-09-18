namespace Miluc.Server.Models.Sap
{
    public class ORDN
    {
        public int DocEntry { get; set; }
        public int? DocNum { get; set; }
        public char? DocType { get; set; }
        public char? CANCELED { get; set; }
        public char? Printed { get; set; }
        public char? DocStatus { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public decimal VatSum { get; set; }
        public decimal DocTotal { get; set; }
        public decimal PaidToDate { get; set; }
        public string? NumAtCard { get; set; }
        public string? Ref1 { get; set; }
        public string? Ref2 { get; set; }
        public string? Comments { get; set; }
        public string? JrnlMemo { get; set; }
        public int? TransId { get; set; }

        public ICollection<RDN1> RDN1 { get; set; } = new List<RDN1>();
    }
}
