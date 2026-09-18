namespace Miluc.Shared.DTOs.Sap.InformesComercial
{
    public class FrecuienciaDeVentasReaderDto
    {
        public string ?CardCode { get; set; }
        public string ?CardName { get; set; }
        public string ?CardFName { get; set; }

        public int ?DocEntry { get; set; }
        public int ?DocNum { get; set; }
        public DateTime ?DocDate{ get; set; }
        public DateTime ? DocDueDate { get; set; }

        //
        public string ? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string ? Almacen { get; set; }
        public string? SlpName { get; set; } 
        public string ? PrioDesc {  get; set; }
        public string ? Telefono1 {  get; set; }
        public string ? Telefono2 {  get; set; }
        public decimal ? Quantity { get; set; }
        public decimal ? SWeight1 { get; set; }
        public decimal? LineTotal { get; set; }


    }
}
