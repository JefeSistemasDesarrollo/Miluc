using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;


namespace Miluc.Server.Interfaces.Nomina
{
    public interface IAfpService
    {
        public Task<(List<AfpReaderDto> Data, int TotalRegistros)> GetAfpAsync(string? filtro = null, int page = 1, int? cantidad = null);
        public Task<AfpReaderDto> CreateAfpAsync(AfpCreateDto afpCreateDto);
    }
}
