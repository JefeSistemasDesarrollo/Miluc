using Miluc.Server.Models;

namespace Miluc.Server.Interfaces.Autorizacion
{
    public interface ITokenService
    {
        //string GenerarToken(Usuario usuario);
        public string GenerarToken(Usuario usuario, List<string> roles, List<string> permisos);
    }
}
