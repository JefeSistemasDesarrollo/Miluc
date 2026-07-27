namespace Miluc.Server.Models.Sap
{
    public class CRD4
    {
    public string CardCode { get; set; }

    public string WTCode { get; set; }

    public decimal Rate { get; set; }

    public virtual OcrdClienteSap OCRD { get; set; }

    public virtual OWHT OWHT { get; set; }
    }
}
