using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models
{
    public class RolPermiso
    {
        [Key]
        public int IdRol { get; set; }
        public Roles Rol { get; set; } = null!;

        public int IdPermiso { get; set; }
        public Permisos Permiso { get; set; } = null!;
    }
}
