using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Autorizacion
{
    public class LoginAccesoRequest
    {
        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 100 caracteres.")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
