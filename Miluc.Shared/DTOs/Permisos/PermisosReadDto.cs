namespace Miluc.Shared.DTOs.Permisos
{
    public class PermisosReadDto
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion {  get; set; }
        public DateTime ?FechaActualizacion {  get; set; }
        public bool Activo { get; set; }
        public List<int> RolesIds { get; set; } = new();
        public List<string> RolesNombre { get; set; } = new();
    }
}
