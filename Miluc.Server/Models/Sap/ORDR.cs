namespace Miluc.Server.Models.Sap
{
    public class ORDR
    {
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public int DocNum { get; set; }
        public int DocEntry { get; set; } //ID del pedido en SAP
        public char? CANCELED { get; set; } //Indica si el pedido fue cancelado ('Y' o 'N')
        public char? DocStatus { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string? NumAtCard { get; set; }
        public decimal? VatSum { get; set; }
        public decimal? DocRate { get; set; }
        public decimal? DocTotal { get; set; }
        public decimal? PaidToDate { get; set; }
        public string? Ref1 { get; set; }
        public string? Ref2 { get; set; }
        public string? Comments { get; set; }
        public string? TransId { get; set; }

        public int? SlpCode { get; set; } //FK OSLP - Vendedor asignado al cliente
        public DateTime CreateDate { get; set; }
        public int? U_Picking { get; set; }
        public string U_PLACAS { get; set; } //fk de piking
        public Int16? U_OrigenPedido { get; set; } 
        public OK1_PICK_TPLACAS? piking { get; set; }
        public ICollection<RDR1>? RDR1 { get; set; } 


        public virtual OcrdClienteSap ? OcrdClienteSap { get; set; }
       
        
      public virtual OSLP ? OSLP { get; set; }
    }
}
