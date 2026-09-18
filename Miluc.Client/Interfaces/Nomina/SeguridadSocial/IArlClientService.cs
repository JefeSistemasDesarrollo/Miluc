using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.ArlDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina.SeguridadSocial
{
    public interface IArlClientService
    {
         Task<ResponseAPI<ArlCreateDto>> CreateArlAsync(ArlCreateDto arlCreateDto);
        Task<ResponseAPI<List<ArlReaderDto>>> GetArlAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        Task<ResponseAPI<ArlReaderDto>> GetByArlAsync(int id);
        Task<ResponseAPI<ArlReaderDto>> UpdateArlAsync(ArlUpdate arlUpdate);
        Task<ResponseAPI<bool>> DeleteArlAsync(int id);

    }
}
