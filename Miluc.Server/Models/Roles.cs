using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models
{
    public class Roles
    {
        [Key]
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ?Descripcion { get; set; }=string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }


        // Navegación
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = [];
        public ICollection<RolPermiso> RolPermisos { get; set; } = [];
    }
}
