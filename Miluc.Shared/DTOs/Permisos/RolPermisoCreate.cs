namespace Miluc.Shared.DTOs.Permisos
{
    public class RolPermisoCreate
    {
        public int IdRol { get; set; }
        public List<int> IdsPermisos { get; set; } = new();
    }
}
