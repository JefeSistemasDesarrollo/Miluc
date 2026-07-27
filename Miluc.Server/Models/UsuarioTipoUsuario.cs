namespace Miluc.Server.Models
{
    public class UsuarioTipoUsuario
    {
      

        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; } = null!;

        public int IdTipoUsuario { get; set; }
        public virtual TipoUsuario TipoUsuario { get; set; } = null!;



    }
}
