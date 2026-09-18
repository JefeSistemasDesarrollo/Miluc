
namespace Miluc.Shared.DTOs.Sap
{
    public class TrazabiliadPorDetalleOrdenReaderDto
    {
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? CardFName { get; set; }
        public string ? ItemCode { get; set; }
        public string ? Dscription { get; set; }
        public string ? unitMsr { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDuedate { get; set; }
        public decimal ?Quantity { get; set; }
        public decimal ?Price{ get; set; }
        public int DocNum { get; set; }



        public DateTime DocDateNota { get; set; }
        public DateTime DocDuedateNotaEntrega { get; set; }
        public decimal? QuantityNotaEntrega { get; set; }


        public DateTime DocDateDevolucion { get; set; }
        public DateTime DocDuedateDevolucion { get; set; }
        public decimal? QuantityDevolucion { get; set; }


        public DateTime DocDateFactura { get; set; }
        public DateTime DocDuedateFactura { get; set; }
        public decimal? QuantityFactura { get; set; }



        public DateTime DocDateNotaCredito { get; set; }
        public DateTime DocDuedateNotaCredito { get; set; }
        public decimal? QuantityNotaCredito { get; set; }


    }
}
