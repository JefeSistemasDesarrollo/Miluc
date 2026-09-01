using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.AfpDto;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina.SeguridadSocial
{
    public interface IAfpClientService
    {
        public Task<ResponseAPI<List<AfpReaderDto>>> GetAfpAsyc(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        public Task<ResponseAPI<AfpCreateDto>> CreateAfpAsync(AfpCreateDto afpCreateDto);
        Task<ResponseAPI<AfpReaderDto>> GetByAfpAsync(int id);
        Task<ResponseAPI<AfpReaderDto>> UpdateAfpAsync(UpdateAfp updateAfp);
        Task<ResponseAPI<bool>> DeleteAfpAsync(int id);

    }
}
