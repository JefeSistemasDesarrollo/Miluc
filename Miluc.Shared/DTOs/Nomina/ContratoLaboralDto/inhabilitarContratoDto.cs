using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Nomina.ContratoLaboralDto
{
    public class InhabilitarContratoDto
    {
        public int ContratoLaboralId { get; set; }

        public bool Activo { get; set; }

        [Required(ErrorMessage = "La observación es obligatoria.")]
        [StringLength(500, ErrorMessage = "La observación no puede superar los 500 caracteres.")]
        public string Observacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de terminación es obligatoria.")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "La fecha de terminación no puede ser futura.")]
        public DateTime? FechaTerminacion { get; set; }
    }
}