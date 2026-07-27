using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Autorizacion
{
    public class LoginResponse
    {
        public bool Requiere2FA { get; set; }

        // Solo cuando login es exitoso sin 2FA
        public UserSession? Session { get; set; }

        // Solo cuando requiere OTP
        public int? IdUsuario { get; set; }
    }
}
