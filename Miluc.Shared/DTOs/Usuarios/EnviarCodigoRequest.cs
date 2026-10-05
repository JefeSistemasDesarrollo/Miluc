using Miluc.Shared.DTOs.Autorizacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Usuarios
{
    public class EnviarCodigoRequest
    {
        public int IdUsuario { get; set; }


        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        public string Correo { get; set; }



        
        //public string? Codigo { get; set; }


        //[OptionalStrongPassword]
        //public string? Password { get; set; }

        //[Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        //public string? ConfirmarPassword { get; set; }


    }
}
