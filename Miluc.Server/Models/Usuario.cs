
using Miluc.Server.Models.AutorizacionModel;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Telefono { get; set; }= string.Empty;
        public byte[]? Foto { get; set; }
        public bool DebeCambiarPassword { get; set; } // Cambiar contraseña en el próximo inicio de sesión

        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = [];
        public byte[] Salt { get; set; } = [];
        public bool Activo { get; set; } = true;
        // Activar/desactivar 2FA
        public bool TwoFactorEnabled { get; set; } 

        //public bool Usuarios {  get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; }  

        // Navegación
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        public ICollection<UsuarioTipoUsuario> UsuarioTipoUsuario = [];


        public ICollection<UsuarioOTP> UsuarioOTP { get; set; } = [];

    }
}
