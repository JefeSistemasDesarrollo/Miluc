using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina.SeguridadSocial
{
    public interface IAfpClientService
    {
        public Task<ResponseAPI<List<AfpReaderDto>>>GetAfpAsyc(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        public Task<ResponseAPI<AfpCreateDto>> CreateAfpAsync(AfpCreateDto afpCreateDto);
    }
}
