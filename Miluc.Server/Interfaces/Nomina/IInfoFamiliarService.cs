using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
//using Miluc.Shared.DTOs.Nomina.PaginacionNomina;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IInfoFamiliarService
    {
        Task<(List<InfoFamiliarReaderDto> data, int CantidadRegistros)> GetFamiliarListAsync(string? filtro = null, int page = 1, int? cantidad = null);
        Task<List<InfoFamiliarReaderDto>> GetFamiliarByIdAsync(int id);
         Task<bool> CreateFamiliarAsync(List<InfoFamiliarCreateDto>   familiar);
         Task <InfoFamiliarReaderDto> UpdateFamiliarAsync( InfoFamiliarUpdateDto familiar);
        Task<bool> DeleteFamiliarAsync(int id); 
         
    }
}
