namespace Miluc.Shared.DTOs.Roles
{
    public class RolUpdateDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
