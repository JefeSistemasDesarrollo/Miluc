using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina.SeguridadSocial
{
    public interface ICajaCompensacionClientService
    {
         Task<ResponseAPI<List<CajaCompensacionReaderDto>>> GetCajaCompensacionAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
         Task<ResponseAPI<CajaCompensacionReaderDto>> CreateCajaAsync(CajaCreateDto createdto);
        Task<ResponseAPI<CajaCompensacionReaderDto>> GetBycajaAsync(int id);
        Task<ResponseAPI<CajaCompensacionReaderDto>> CajaUpdate(CajaUpdate cajaUpdate);
    }
}
