using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.ArlDto;
using Miluc.Shared.Models.Response;


namespace Miluc.Server.Interfaces.Nomina
{
    public interface IArlService
    {
     
        public Task<(List<ArlReaderDto> Data, int TotalRegistros)> GetArlAsync(string? filtro = null, int page = 1, int? cantidad = null);
        public Task<ArlReaderDto> CreateArlAsync(ArlCreateDto arlCreateDto);
        public Task<ArlReaderDto> GetByArlAsync(int id);
        public  Task<ArlReaderDto> UpdateArlAsync(ArlUpdate arlUpdate);
        public  Task<bool> DeleteArlAsync(int id);


    }
}
