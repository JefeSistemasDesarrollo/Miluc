using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Sap.Pedidos
{
    public class PedidoUpdateDto
    {
        public int DocEntry { get; set; } //ID del pedido en SAP
        //public string? DocNum { get; set; }
        public string CardCode { get; set; } = string.Empty;
        [MaxLength(100, ErrorMessage = "El numero de referencia excede el límite de caracteres permitido")]
        public string ? NumAtCard { get; set; } = string.Empty;
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string? Reference2 { get; set; }
        //public char? CANCELED { get; set; } //Indica si el pedido fue cancelado ('Y' o 'N')

        [MaxLength(254, ErrorMessage = "El comentario excede el límite de caracteres permitido")]
        public string? Comments { get; set; }
        public int? U_OrigenPedido { get; set; }
        public string? U_Responsable { get; set; }
        public int? SalesPersonCode { get; set; }//vendedor
        public string? JournalMemo { get; set; }
        public decimal U_CantidadKg { get; set; }
        public decimal U_UnidadSal { get; set; }
        public List<PedidoCrearDetalleLineaDTO> DocumentLines { get; set; } = new();
    }
}
