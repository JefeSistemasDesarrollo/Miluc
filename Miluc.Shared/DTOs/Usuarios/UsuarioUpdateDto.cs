using Miluc.Shared.DTOs.Autorizacion;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Miluc.Shared.DTOs.Usuarios
{
    public class UsuarioUpdateDto
    {


        [Required]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nombres son obligatorios")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Apellidos son obligatorios")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Telefono { get; set; }= string.Empty;

        public byte[]? Foto { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public bool DebeCambiarPassword { get; set; } // Cambiar contraseña en el próximo inicio de sesión

        public int ? CodVendedorSAP { get; set; }

        // La contraseña es opcional en el Update

        [OptionalStrongPassword]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarPassword { get; set; }

        public bool Activo { get; set; }

        // Listas actualizadas para las tablas intermedias
        public List<int> TiposUsuarioIds { get; set; } = new();
        public List<int> RolesIds { get; set; } = new();


    }
}
