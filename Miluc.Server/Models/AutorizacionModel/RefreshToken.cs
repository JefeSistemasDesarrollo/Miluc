using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miluc.Server.Models.AutorizacionModel
{
    public class RefreshToken
    {
        [Key]
        public Guid IdRefreshToken { get; set; }

        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        public byte[] TokenHash { get; set; } = [];

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Expires { get; set; }

        public DateTime? Revoked { get; set; }

        //public Guid? ReplacedByToken { get; set; }

        public string? RemoteIpAddress { get; set; }

        public string? UserAgent { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
