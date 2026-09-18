using Miluc.Shared.DTOs.Sap.Articulos;
using Miluc.Shared.DTOs.Sap.Devolucion;
using Miluc.Shared.DTOs.Sap.Factura;
using Miluc.Shared.DTOs.Sap.NotaCredito;
using Miluc.Shared.DTOs.Sap.NotaDeEntrega;

namespace Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta
{
    public class TrazabilidadOrdenReaderDto
    {
        //   //ordenes de venta 

        //public string? CardCode { get; set; }
        //public string? CardName { get; set; }
        //public int DocNum { get; set; }
        public int DocEntry { get; set; } 
        //ID del pedido en SAP
                                          //   public char? CANCELED { get; set; } //Indica si el pedido fue cancelado ('Y' o 'N')
                                          //   public char? Printed { get; set; }
                                          //   public char? DocStatus { get; set; }
                                          //   public DateTime DocDate { get; set; }
                                          //   public DateTime DocDueDate { get; set; }
                                          //   public DateTime TaxDate { get; set; }
                                          //   //public string? Address { get; set; }
                                          //   //public string? Address2 { get; set; }
                                          //   public string? NumAtCard { get; set; }
                                          //   public decimal? VatSum { get; set; }
                                          //   public decimal? DocRate { get; set; }
                                          //   public decimal? DocTotal { get; set; }
                                          //   public decimal? PaidToDate { get; set; }
                                          //   public string? Ref1 { get; set; }
                                          //   public string? Ref2 { get; set; }
                                          //   public string? Comments { get; set; }
                                          //   //public string? TransId { get; set; }
                                          //   //public int? SlpCode { get; set; } //FK OSLP - Vendedor asignado al cliente
                                          //   public DateTime CreateDate { get; set; }
                                          //   public int? U_Picking { get; set; }
                                          //   public string? U_PLACAS { get; set; } //fk de piking
                                          //   //public Int16? U_OrigenPedido { get; set; }

        //   public ICollection<DetalleOrdenReaderDto>?detalleOrden {  get; set; }


        //   //aca voy a realizar la nota de enrega 
        //public ICollection<Dln1ReaderDto>detalleNota{ get; set;  }


        public List<OdlnReaderDto> ?OdlnReader { get; set; }=new List<OdlnReaderDto>();
        public List<OinvReaderDto> ?OinvReader { get; set; }=new List<OinvReaderDto>();

        public List<OrdnReaderDto> ?OrdnReader { get; set; }=new List<OrdnReaderDto>();

        public List <OrinReaderDto> ?orinReaders { get; set; }=new List<OrinReaderDto>();

        //public OdlnReaderDto OdlnReader { get; set; }

        //public OinvReaderDto OinvReader { get; set; }

        public List<OinvReaderDto>? OinvOrders { get; set; }=new List<OinvReaderDto> ();
    }
}
