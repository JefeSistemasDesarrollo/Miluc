using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.AfpDto;
using Miluc.Shared.Models.Response;


namespace Miluc.Server.Interfaces.Nomina
{
    public interface IAfpService
    {
        public Task<(List<AfpReaderDto> Data, int TotalRegistros)> GetAfpAsync(string? filtro = null, int page = 1, int? cantidad = null);
        public Task<AfpReaderDto> CreateAfpAsync(AfpCreateDto afpCreateDto);
        public Task<AfpReaderDto> GetByAfpAsync(int id);
        public Task<AfpReaderDto> UpdateAfpAsync(UpdateAfp updateAfp);
        Task<bool> DeleteAfpAsync(int id);
    }
}

