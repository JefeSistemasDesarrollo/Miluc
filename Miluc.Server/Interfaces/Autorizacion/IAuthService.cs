using Miluc.Shared.DTOs.Autorizacion;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Autorizacion
{
    public interface IAuthService
    {
        // Método principal para validar credenciales
        Task<ResponseAPI<UserSession?>> LoginAsync(LoginAccesoRequest request, string ipAddress, string userAgent);


        //cambiar contraseña 
        Task<ResponseAPI<bool>> CambiarPasswordAsync(CambiarPasswordRequest request);

        // Método para cerrar sesión (si necesitas lógica extra en DB)

        Task<bool> LogoutAsync(string refreshToken);
        //Task<string> SeedAsync(); // Nuevo método
        Task<ResponseAPI<UserSession?>> RefreshTokenAsync(string tokenPlano, string ipAddress, string userAgent);
        //Task<UserSession?> RefreshTokenAsync(string tokenPlano, string ipAddress, string userAgent);

        Task<bool> RegisterAsync(string username, string password, string email);
        Task<ResponseAPI<UserSession?>> VerifyOtpAsync(int idUsuario, string codigo, string ipAddress, string userAgent);

        //bool HasPermission(string permiso);

    }
}
