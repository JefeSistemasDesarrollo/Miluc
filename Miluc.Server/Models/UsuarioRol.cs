namespace Miluc.Server.Models
{
    public class UsuarioRol
    {
       
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int IdRol { get; set; }
        public Roles Rol { get; set; } = null!;
    }
}
