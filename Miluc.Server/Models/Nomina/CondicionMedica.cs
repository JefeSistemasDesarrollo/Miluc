namespace Miluc.Server.Models.Nomina
{
    public class CondicionMedica
    {
        public int CondicionMedicaId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public virtual ICollection<CondicionMedicaDetalle> CondicionesMedicasDetalle { get; set; } = new List<CondicionMedicaDetalle>();
    }
}
