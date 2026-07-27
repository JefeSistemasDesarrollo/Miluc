namespace Miluc.Shared.DTOs.Sap.Pedidos
{
    public class OrdersReaderDetalleDto
    {

        public int DocEntry { get; set; }
        public int ? LineNum { get; set; }


        public string ? ItemCode { get; set; } = string.Empty;

        public string? Dscription { get; set; } = string.Empty;

        public decimal? Quantity { get; set; }
        public decimal? Rate { get; set; }

        public decimal? Price { get; set; }

        public decimal? LineTotal { get; set; }

        public decimal? VatSum { get; set; }

        public decimal? Weight1 { get; set; }

        public string? TaxCode { get; set; } = string.Empty;

        public string? UnitMsr { get; set; } = string.Empty;

        public string ? WhsCode { get; set; } = string.Empty;
    }
}
