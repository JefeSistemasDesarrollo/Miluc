namespace Miluc.Server.Models.Nomina
{
    public class CentroCostos
    {
        public int CentroCostoId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Activo { get; set; }
        public ICollection<ContratoLaboral> ContratoLaboralModel { get; set; }
    }
}
