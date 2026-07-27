using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Usuarios
{
    public class TipoUsuarioCreateDto
    {
        [Required(ErrorMessage = "El nombre del tipo es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
