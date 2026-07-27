namespace Miluc.Server.Models.Sap
{
    public class BPCO_MU
    {
        public string Code { get; set; }     // codigo
        public string Name { get; set; }     // Nombre 
        public ICollection<CRD1> CRD1 { get; set; } // Relación con clientes (si es necesario)
    }
}
