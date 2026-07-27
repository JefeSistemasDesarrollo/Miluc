using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Permisos
{
    public class PermisosUpdateDto
    {


        [Required]  
        public int IdPermiso { get; set; }


        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [MinLength(5, ErrorMessage = "El campo debe tener un minimo de 8 caracteres")]
        [MaxLength(100, ErrorMessage = "El campo debe tener un minimo de 8 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        // Navegación

        public List<int> RolesIds { get; set; } = new();

        //public List<string> RolesNombre { get; set; } = new();

    }
}
