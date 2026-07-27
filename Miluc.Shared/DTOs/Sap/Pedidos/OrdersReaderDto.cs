using Miluc.Shared.DTOs.Sap.Articulos;

namespace Miluc.Shared.DTOs.Sap.Pedidos
{
    public class OrdersReaderDto
    {
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? CardFName { get; set; }
        public string? Domicilio { get; set; }
        public string? ListName { get; set; }
        public string? NitCliente { get; set; }
        public string? DocNum { get; set; }
        public string? Balance { get; set; }
        public int ?SlpCode { get; set; } 
        public string SlpName { get; set; } = string.Empty;
        public int? DocEntry { get; set; } //ID del pedido en SAP
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
        public string? Comments { get; set; } = string.Empty;
        public string? TransId { get; set; }
        // public int ? SlpCode { get; set; } //Código del vendedor
        public DateTime? CreateDate { get; set; }
        public int? U_Picking { get; set; }
        public string? U_PLACAS { get; set; }
        public Int16? U_OrigenPedido { get; set; }
        public List<ArticuloPorListaDePreciosDto>? detalle { get; set; }        
    }
}
