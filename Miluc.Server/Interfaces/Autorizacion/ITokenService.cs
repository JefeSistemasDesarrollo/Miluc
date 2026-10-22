using Miluc.Server.Models;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto;

namespace Miluc.Server.Interfaces.Autorizacion
{
    public interface ITokenService
    {
        //string GenerarToken(Usuario usuario);
        public string GenerarToken(Usuario usuario, List<string> roles, List<string> permisos, List<AsignarFiltrosReaderDto> filtros);
    }
}
