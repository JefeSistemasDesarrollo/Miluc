using System.Collections;

namespace Miluc.Server.Models.Nomina
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public string Nit { get; set; }
        public string NombreEmpresa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

         public ICollection<ContratoLaboral> ContratoLaboralModel { get; set; } // Navigation a ContratoLaboral
       
    }
}
