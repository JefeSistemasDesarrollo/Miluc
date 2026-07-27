using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina.SeguridadSocial
{
    public interface IArlClientService
    {
        public Task<ResponseAPI<ArlCreateDto>> CreateArlAsync(ArlCreateDto arlCreateDto);
        public Task<ResponseAPI<List<ArlReaderDto>>> GetArlAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
    }
}
