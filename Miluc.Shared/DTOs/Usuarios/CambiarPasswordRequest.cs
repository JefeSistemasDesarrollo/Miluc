using Miluc.Shared.DTOs.Autorizacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Usuarios
{
    public class CambiarPasswordRequest
    {
        public int IdUsuario { get; set; }



        [OptionalStrongPassword]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarPassword { get; set; }

    }

}
