namespace Miluc.Server.Models.Nomina
{
    public class ContratoLaboral
    {
        public int ContratoLaboralId { get; set; }
        public int EmpresaId { get; set; }
        public int EmpleadoId { get; set; }
        public int TipoContratoId { get; set; }
        public DateTime FechaCreacion {  get; set; }
        public int ContratoLaboralDetalleId { get; set; }

        public Empresa Empresa { get; set; } // Navigation a Empresa
        public TipoContrato TipoContrato { get; set; } // Navigation a TipoContrato

        public Empleado Empleado { get; set; } // Navigation a Empleado
        public ICollection<ContratoLaboralDetalle> ContratoLaboralDetalle { get; set; } // Navigation a ContratoLaboralDetalle
      


    }
}
