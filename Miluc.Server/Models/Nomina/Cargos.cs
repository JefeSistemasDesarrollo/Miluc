namespace Miluc.Server.Models.Nomina
{
    public class Cargo
    {
        public int CargoId { get; set; } 
        public string CargoNombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Activo { get; set; }

        public ICollection<ContratoLaboralDetalle> ContratoLaboralDetalle { get; set; } = new List<ContratoLaboralDetalle>();
    }
}