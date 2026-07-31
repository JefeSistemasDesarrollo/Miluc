using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto
{
    public class InfoFamiliarUpdateDto
    {
        public int InformacionFamiliarId { get; set; }
        [Required (ErrorMessage ="NUmero de Documento es obligatorio")]
        public string Documento { get; set; } = string.Empty;
        public int EmpleadoId { get; set; } // Foreign key de Empleado
        [Required(ErrorMessage =" Campo nombre Completo es Obliagatorio")]

        public string NombreCompleto { get; set; }

        [Required(ErrorMessage ="Debe seleccionar un parentesco")]
        public int ParentescoId { get; set; } // Foreign key de Parentesco
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage ="Debe seleccionar opcion ")]
        
        public bool? ViveConEmpleado { get; set; }
        [Required(ErrorMessage = "Debe seleccionar opcion ")]
        public bool? DependeEconomicamente { get; set; }
        [Required(ErrorMessage = "Debe seleccionar opcion ")]
        public bool? PersonaaCargo { get; set; }
        public bool Activo { get; set; }
        //public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
