namespace Miluc.Shared.DTOs.Usuarios
{
    public class TipoUsuarioReadDto
    {
        public int IdTipoUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
