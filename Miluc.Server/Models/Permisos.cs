using System.ComponentModel.DataAnnotations;

namespace Miluc.Server.Models
{
    public class Permisos
    {
        [Key]
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ?Descripcion { get; set; }
        public bool Activo { get; set; }=true;

        public DateTime FechaCreacion { get; set; }
        public DateTime ?FechaActualizacion { get; set; }

        // Navegación
        public ICollection<RolPermiso> RolPermisos { get; set; } = [];
    }
}
