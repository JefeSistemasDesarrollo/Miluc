using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Usuarios
{
    public class ConfirmarCodigoSeguridad
    {
        [Required(ErrorMessage = "El Codigo de seguridad es obligatorio")]
        public string? Codigo { get; set; }
    }
}
