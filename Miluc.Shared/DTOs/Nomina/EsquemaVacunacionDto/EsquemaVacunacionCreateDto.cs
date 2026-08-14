using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto
{
    public class CreateEsquemaVacunacionDto
    {
        
        public int EmpleadoId { get; set; }
        public int VacunaId { get; set; }
        public string NombreVacuna { get; set; }
        public DateTime? FechaVacuna { get; set; } 
        public DateTime? FechaCreacion { get; set; } 
        public bool? Activo { get; set; } 



    }
}
