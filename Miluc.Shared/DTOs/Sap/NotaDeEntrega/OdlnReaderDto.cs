namespace Miluc.Shared.DTOs.Sap.NotaDeEntrega
{
    public class OdlnReaderDto
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public char DocType { get; set; }
        public char CANCELED { get; set; }
        public char Printed { get; set; }
        public char DocStatus { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }

        public string CardCode { get; set; }
        public string CardName { get; set; }

        //  public Int16 SlpCode { get; set; }
        public string? Comments { get; set; }

        public decimal DocTotal { get; set; }
        public decimal PaidToDate { get; set; }
        public string? JrnlMemo { get; set; }

        public int  ? BaseEntry { get; set; } = 0;



        public ICollection<Dln1DetalleReaderDto> detalleNotaEntrega { get; set; } = new List<Dln1DetalleReaderDto>();


    }
}
