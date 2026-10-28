using System;
using System.ComponentModel.DataAnnotations;
using Miluc.Shared.DTOs.Nomina.Validations
    ;

namespace Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto
{
    public class InfoFamiliarCreateDto
    {
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "Campo documento es obligatorio.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El documento solo puede contener números.")]
        [MinLength(4, ErrorMessage = "El documento debe tener al menos 4 caracteres.")]
        [MaxLength(20, ErrorMessage = "El documento no puede tener más de 20 caracteres.")]
        public string Documento { get; set; } = string.Empty;

        public string? NombreEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre completo no puede superar los 150 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parentesco es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un parentesco válido.")]
        public int? ParentescoId { get; set; }

        public string? NombreParentesco { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "La fecha de nacimiento no puede ser futura.")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debe especificar si vive con el empleado.")]
        public bool? ViveConEmpleado { get; set; }

        [Required(ErrorMessage = "Debe especificar si depende económicamente.")]
        public bool? DependeEconomicamente { get; set; }

        [Required(ErrorMessage = "Debe especificar si es una persona a cargo.")]
        public bool? PersonaaCargo { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}