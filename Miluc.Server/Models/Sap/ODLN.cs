using System.Text.Json.Serialization;

namespace Miluc.Server.Models.Sap
{
    public class ODLN
    {
        public int DocEntry {  get; set; }
        public int DocNum {  get; set; }
        public char DocType { get; set; }
        public char CANCELED { get; set; }
        public char Printed { get; set; }
        public char DocStatus { get; set; }
        public DateTime DocDate {  get; set; }
        public DateTime DocDueDate {  get; set; }

        public string CardCode { get; set; }
        public string CardName { get; set; }

      //  public Int16 SlpCode { get; set; }
        public string? Comments { get; set; }

        public decimal DocTotal {  get; set; }
        public decimal PaidToDate {  get; set; }
        public string ?JrnlMemo {  get; set; }

        //vendedor 
        //public OSLP OSLP {  get; set; }
        [JsonIgnore]
       public ICollection<DLN1> DLN1 {  get; set; }

    }
}
