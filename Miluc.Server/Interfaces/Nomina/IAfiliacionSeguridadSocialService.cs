using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IAfiliacionSeguridadSocialService
    {
        Task<(List<AfiliacionSeguridadSocialreaderDto> Data, int CantidadRegistros)> GetAfiliacionSeguridadSocialAsync(string? filtro = null, int page = 1, int? cantidad = null);
        Task<AfiliacionSeguridadSocialreaderDto> GetAfiliacionSeguridadSocialByIdAsync(int afiliacionId);
        Task<bool> UpsertAfiliacionAsync(UpdateAFiliacionDto dto);
    }
}