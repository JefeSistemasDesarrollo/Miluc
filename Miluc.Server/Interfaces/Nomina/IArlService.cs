using Miluc.Shared.DTOs.Nomina.Arl.Dto;


namespace Miluc.Server.Interfaces.Nomina
{
    public interface IArlService
    {
     
        public Task<(List<ArlReaderDto> Data, int TotalRegistros)> GetArlAsync(string? filtro = null, int page = 1, int? cantidad = null);
        public Task<ArlReaderDto> CreateArlAsync(ArlCreateDto arlCreateDto);
    }
}
