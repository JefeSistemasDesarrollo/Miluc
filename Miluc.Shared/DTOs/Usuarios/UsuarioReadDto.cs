namespace Miluc.Shared.DTOs.Usuarios
{
    public class UsuarioReadDto
    {  
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public string Email { get; set; } = null!;
        public string Telefono { get; set; }= string.Empty;
        public string Password { get; set; } = string.Empty; 
        public string ConfirmarPassword { get; set; } = string.Empty;
        public byte[]? Foto { get; set; }
        public bool DebeCambiarPassword { get; set; } // Cambiar contraseña en el próximo inicio de sesión
        public bool Activo { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime FechaCreacion { get; set; }
        // Listas de strings para mostrar nombres en la interfaz (ej: "Admin, Editor")
        public List<string> NombresRoles { get; set; } = new();
        public List<string> NombresTiposUsuario { get; set; } = new();
        public List<int> TiposUsuarioIds { get; set; } = new();
        public List<int> RolesIds { get; set; } = new();
    }
}
