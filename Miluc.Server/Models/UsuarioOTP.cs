using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miluc.Server.Models
{
    public class UsuarioOTP
    {
        [Key]
        public int IdOtp { get; set; }
        public int IdUsuario { get; set; }
        public byte[] CodigoHash { get; set; } = [];
        public DateTime Expira { get; set; }
        public bool Usado { get; set; } = false;
        public int Intentos { get; set; } = 0;

        public string? RemoteIpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;
    }
}
