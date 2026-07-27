namespace Miluc.Server.Models.Sap
{
    public class OBPP
    {
        public int PrioCode { get; set; }
        public string? PrioDesc { get; set; }
        public string? U_Camion { get; set; }
        public string? U_Cuenta { get; set; }
        public string? U_SerieDev { get; set; }
        public string? U_SerieEntregas { get; set; }
        public string? U_SerieNC { get; set; }
        public string? U_SeriePagos { get; set; }
        public string? U_SerieTrans { get; set; }
        public string? U_Sucursal { get; set; }

        public ICollection<OcrdClienteSap> OCRD { get; set; }

    }
}
