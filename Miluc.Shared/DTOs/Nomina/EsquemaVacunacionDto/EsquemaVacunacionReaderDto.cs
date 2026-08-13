using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto
{
    public class EsquemaVacunacionReaderDto
    {
        public int EsquemaVacunacionId { get; set; }
        public int EmpleadoId { get; set; } = 0;
        public string NombreEmpleado { get; set; } = string.Empty;

        public int? VacunaId { get; set; }
        public string NombreVacuna { get; set; }
        public bool? Activo { get; set; }    
        public string Documento { get; set; }
        public DateTime? FechaVacuna { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
