using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto
{
    public class ContratoLaboralDetalleDto
        
    {
        public int ContratoLaboralId { get; set; }
        public int ContratoLaboralDetalleId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string CentroCosto { get; set; } = string.Empty;
        public decimal Salario { get; set; }

        


    }
}
