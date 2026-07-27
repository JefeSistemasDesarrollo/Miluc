namespace Miluc.Server.Models.Sap
{
    public class HBT_REGIMTRIB
    {
        public string? code { get; set; }
        public string? Name { get; set; }
       
        public ICollection<OcrdClienteSap> OCRD { get; set; }
    }
}
