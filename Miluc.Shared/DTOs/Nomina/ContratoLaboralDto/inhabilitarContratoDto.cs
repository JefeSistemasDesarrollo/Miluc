using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Nomina.ContratoLaboralDto
{
    public class InhabilitarContratoDto
    {
        public int ContratoLaboralId { get; set; }
        public bool Activo { get; set; }
        [Required(ErrorMessage = "La observación es requerida.")]
        public string Observacion { get; set; }
        [Required(ErrorMessage = "La fecha de terminación es requerida.")]
        [DataType(DataType.Date)]
        public DateTime? FechaTerminacion { get; set; }
    }
}