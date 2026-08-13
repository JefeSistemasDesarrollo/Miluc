using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto
{
    public class InfoFamiliarReaderDto
    {
        public int InformacionFamiliarId { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El documento solo puede contener números")]
        [Required(ErrorMessage = " Campo documento es obligatorio")]

        public string Documento { get; set; }
        public int EmpleadoId { get; set; } // Foreign key de Empleado
        public string NombreEmpleado { get; set; }
        public string NombreCompleto { get; set; }
        public int ParentescoId { get; set; } // Foreign key de Parentesco
        public string Parentesco { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool ViveConEmpleado { get; set; }
        public bool DependeEconomicamente { get; set; }
        public bool PersonaaCargo { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }


    }
}
