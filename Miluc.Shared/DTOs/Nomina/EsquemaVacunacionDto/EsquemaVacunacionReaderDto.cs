using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EsquemaVacunbacionDto
{
    public class EsquemaVacunacionReaderDto
    {
        public int EsquemaVacunacionId { get; set; }
        public int EmpleadoId { get; set; } = 0;
        public int VacunaId { get; set; }
        public string VacunaName { get; set; }
        public string NombreEmpleado { get; set; }
        public DateTime FechaVacuna { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
