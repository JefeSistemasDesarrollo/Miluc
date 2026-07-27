namespace Miluc.Server.Models.Sap
{
    public class HBT_REGIMENFISCAL
    {
        public string ? Code { get; set; }
        public string ? Name { get; set; }
        public ICollection<OcrdClienteSap> OCRD { get; set; }

    }
}
