namespace Miluc.Shared.DTOs.Roles
{
    public class RolReadDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; }

        public int CantidadRoles { get; set; }

    }
}
