using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.ContratoLaboralDto
{
    public class ContratoLabralreaderDto
    {
        public int EmpleadoId { get; set; }//fk de empleado
        public string NombreEmpleado { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;


        public int ContratoLaboralId { get; set; }
        public int EmpresaId { get; set; } //fk de empresa
        public string NombreEmpresa { get; set; }= string.Empty;
        public string Nit { get; set; }
        public int ContratoLaboralDetalleId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string CentroCosto { get; set; } = string.Empty;
        public decimal Salario { get; set; }


        public int TipoContratoId { get; set; } //fk de tipo
        public string NombreTipoContrato { get; set; } = string.Empty;  

        public DateTime FechaCreacion { get; set; }
        public bool? Activo{ get; set; }




       
    }
}
