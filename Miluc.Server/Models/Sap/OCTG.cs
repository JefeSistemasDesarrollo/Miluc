namespace Miluc.Server.Models.Sap
{
    public class OCTG
    {
        public Int16 GroupNum { get; set; }
      //  public string? GroupName { get; set; }
        public string? PymntGroup { get; set; }
        public char? PayDuMonth { get; set; }
        public Int16? ExtraMonth { get; set; }
        public Int16? ExtraDays { get; set; }
        public Int16? PaymntsNum { get; set; }
        public Int16? ListNum { get; set; }
        public char? Payments { get; set; }
        public char? BslineDate { get; set; }
        public Int16? InstNum { get; set; }
        public ICollection<OcrdClienteSap>? OCRD { get; set; }

    }
}
