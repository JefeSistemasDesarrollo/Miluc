using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IAfiliacionSeguridadSocialService
    {
        public Task<(List<AfiliacionSeguridadSocialreaderDto> Data, int CantidadRegistros)> GetAfiliacionSeguridadSocialAsync(string? filtro = null, int page = 1, int? cantidad = null);
        public Task<AfiliacionSeguridadSocialreaderDto> GetAfiliacionSeguridadSocialByIdAsync(int afiliacionId);
        public Task<bool> UpsertAfiliacionAsync(UpdateAFiliacionDto dto);
    }
}