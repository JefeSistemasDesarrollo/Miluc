using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Miluc.Shared.DTOs.Sap.Pedidos
{
    public class PedidoCreateDto
    {
        //IdSolicitud
      
        public string CardCode { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "El numero de referencia excede el límite de caracteres permitido")]
        public string NumAtCard {  get; set; } = string.Empty;
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }

        public string? Reference2 { get; set; }

        [MaxLength(254, ErrorMessage = "El comentario excede el límite de caracteres permitido")]
        public string? Comments { get; set; }
        public int? U_OrigenPedido { get; set; }
        public string? U_Responsable { get; set; }
        public int? SalesPersonCode { get; set; }//vendedor

        public string? JournalMemo { get; set; }

        public decimal U_CantidadKg { get; set; }

        public decimal U_UnidadSal { get; set; }


        [Required(ErrorMessage = "El campo U_WUID es obligatorio.")]
        public string ? U_WUID { get; set; }


        //[JsonIgnore]
        //  public Guid IdSolicitud { get; set; }


        // public string ? Printed {  get; set; }

        //[JsonIgnore]
        //public int IdUsuario { get; set; }

        public List<PedidoCrearDetalleLineaDTO> DocumentLines { get; set; } = new();
    }
}
