using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Nomina.ContratoLaboralDto
{
    public class UpdateContratoDto
    {
        [Required]
        public int ContratoLaboralId { get; set; } // ¡Vital para saber qué actualizar!

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una empresa")]
        public int EmpresaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de contrato")]
        public int TipoContratoId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        // Si la fecha de finalización es opcional (para contratos indefinidos), quita el [Required]
        [DataType(DataType.Date)]
        public DateTime? FechaFinalizacion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cargo")]
        public int CargoId { get; set; }

        [Required(ErrorMessage = "Agregar un centro de costo")]
        public string CentroCosto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El salario es obligatorio")]
        public decimal? Salario { get; set; }
    }
}