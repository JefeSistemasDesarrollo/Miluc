using Miluc.Shared.DTOs.Autorizacion;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces
{
    public interface IAuthClientService
    {
        Task<ResponseAPI<UserSession>> Login(LoginAccesoRequest loginRequest);
        Task<ResponseAPI<UserSession>> VerifyOtp(VerifyOtpRequest request);
        Task<ResponseAPI<bool>> CambiarPassword(CambiarPasswordRequest request);

        UserSession? CurrentSession { get; }
        Task InitializeAsync();
        Task Logout();
        Task<ResponseAPI<UserSession>> RefreshSession();
        bool HasPermission(string permiso);


    }
}
