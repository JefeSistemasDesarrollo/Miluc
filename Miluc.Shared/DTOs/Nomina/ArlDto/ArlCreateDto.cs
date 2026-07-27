using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.Arl.Dto
{
    public class ArlCreateDto
    {
        [Required(ErrorMessage = "El nombre de la ARL es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El código de la ARL es obligatorio.")]
        public string Codigo { get; set; }
        public bool Activo { get; set; }
    }
}
