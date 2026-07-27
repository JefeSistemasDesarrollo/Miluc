using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EpsDto
{
    public class EpsCreateDto
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Código es obligatorio.")]
        public string Codigo { get; set; } = string.Empty;
        public bool Activo { get; set; } 

    }
}
