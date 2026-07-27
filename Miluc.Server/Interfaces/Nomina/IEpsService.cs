using Miluc.Shared.DTOs.Nomina.EpsDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IEpsService
    {
         public Task<(List<EpsReaderDto> Data, int TotalRegistros)> GetEpsAsync(string? filtro = null, int page = 1, int? cantidad = null);
        public Task<EpsReaderDto> CreateEpsAsync(EpsCreateDto epsCreateDto);
        
    }
}
