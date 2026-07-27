using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto
{
    public class InfoFamiliarCreateDto
    {
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "Campo documento es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El documento solo puede contener números")]
        [MinLength(4, ErrorMessage = "El documento debe tener al menos 4 caracteres.")]
        public string Documento { get; set; } = string.Empty;

        public string? NombreEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Parentesco es obligatorio")]
        public int? ParentescoId { get; set; }

        public string? NombreParentesco { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "La fecha de nacimiento no puede ser futura")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debe especificar si vive con el empleado")]
        public bool? ViveConEmpleado { get; set; }

        [Required(ErrorMessage = "Debe especificar si depende económicamente")]
        public bool? DependeEconomicamente { get; set; }

        [Required(ErrorMessage = "Debe especificar si es una persona a cargo")]
        public bool? PersonaaCargo { get; set; } 

        public bool Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}