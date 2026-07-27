namespace Miluc.Server.Models.Sap
{
    public class OSTC
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public char ValidForAR { get; set; }
        public char Lock { get; set; }
        public ICollection<OITM> OITM { get; set; }
    }
}
