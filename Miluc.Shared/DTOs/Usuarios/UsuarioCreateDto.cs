using Miluc.Shared.DTOs.Sap.DireccionCliente;
using Miluc.Shared.DTOs.Sap.Impuesto;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Usuarios
{
    public class UsuarioCreateDto
    {

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        public string Email { get; set; } = string.Empty;


        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Debe contener mayúscula, minúscula, número y carácter especial.")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = string.Empty;

        // Propiedad auxiliar para la carga de imagen en Blazor
        public string? FotoBase64 { get; set; }

        // Propiedad que el servicio guardará en la base de datos
        public byte[]? Foto { get; set; }

        public bool TwoFactorEnabled { get; set; } = true;

        public bool DebeCambiarPassword { get; set; } = false; // Cambiar contraseña en el próximo inicio de sesión


        public int ? CodVendedorSAP { get; set; } 
        // Listas de IDs para las tablas intermedias (UsuarioRol y UsuarioTipoUsuario)
        public List<int> RolesIds { get; set; } = new();
        public List<int> TiposUsuarioIds { get; set; } = new();

        // Propiedad auxiliar para el Select simple en la UI
        public int IdTipoUsuario { get; set; }

    }
}

