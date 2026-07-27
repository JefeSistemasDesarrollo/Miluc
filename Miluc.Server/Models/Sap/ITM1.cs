namespace Miluc.Server.Models.Sap
{
    public class ITM1
    {
        public string ItemCode { get; set; }
        public Int16? PriceList { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public OITM OITM { get; set; } = new OITM();
        public OPLN OPLN { get; set; } = new OPLN();
    }
}
