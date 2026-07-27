using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models
{
    public class TipoUsuario
    {
        [Key]
        public int IdTipoUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; }


        // Relaciones
        public ICollection<UsuarioTipoUsuario> UsuarioTipoUsuario = [];

    }
}
