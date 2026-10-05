using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.CargosDto
{
    public class UpsertCargos
    {
        public int CargoId { get; set; }
        [Required(ErrorMessage = "El nombre del cargo es obligatorio.")]
        public string CargoNombre { get; set; }
        public bool Activo { get; set; }
    }
}
