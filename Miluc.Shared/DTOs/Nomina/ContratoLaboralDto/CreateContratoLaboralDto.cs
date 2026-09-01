using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Nomina.ContratoLaboralDto
{
    public class CreateContratoLaboralDto
    {
        // Cabecera tabla contrato laboral
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una empresa")]
        public int EmpresaId { get; set; } // FK de empresa

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de contrato")]
        public int TipoContratoId { get; set; } // FK de tipo

        public int EmpleadoId { get; set; } // FK de empleado
        public string NombreEmpledo { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }

        // Detalle - tabla detalle contrato
        public int ContratoLaboralId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "Fecha inicio  no puede ser mayor")]
        public DateTime? FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es obligatoria")]
        [DataType(DataType.Date)]
        
        public DateTime? FechaFinalizacion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cargo")]
        public int CargoId { get; set; }

        [Required(ErrorMessage = "Agregar un centro de costo")]
        public string CentroCosto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El salario es obligatorio")]
        [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "El salario debe ser un número válido (hasta 2 decimales)")]
        public decimal? Salario { get; set; }
    }
}