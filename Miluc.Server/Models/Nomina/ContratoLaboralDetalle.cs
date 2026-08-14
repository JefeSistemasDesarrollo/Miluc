

namespace Miluc.Server.Models.Nomina
{
    public class ContratoLaboralDetalle
    {
        public int ContratoLaboralDetalleId { get; set; } // Foreign key a ContratoLaboral
        public int ContratoLaboralId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string CentroCosto { get; set; } = string.Empty;
        public decimal Salario { get; set; }
        // public int ContratoLaboralDetlleId { get; set; } // Foreign key a ContratoLaboral
        public ContratoLaboral ContratoLaboral { get; set; } // Navigation a ContratoLaboral
    }
}