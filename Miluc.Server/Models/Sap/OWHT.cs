namespace Miluc.Server.Models.Sap
{
    //
    public class OWHT
    {
        public string ? WTCode { get; set; }
        public string ? WTName { get; set; }
        public char ? Type { get; set; }

        public ICollection<CRD4> CRD4 { get; set; }

    }
}
