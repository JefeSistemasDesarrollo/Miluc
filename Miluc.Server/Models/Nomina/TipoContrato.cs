using System.Collections;

namespace Miluc.Server.Models.Nomina
{
    public class TipoContrato
    {
        public int TipoContratoId { get; set; }
        public string NombreContrato { get; set; }

        // Navigation a ContratoLaboral
        public ICollection<ContratoLaboral> ContratoLaboralModel { get; set; }
    }
}