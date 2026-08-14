using Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto;
using Miluc.Client.Interfaces.Nomina;


using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IEsquemaVacunacionClienteService
    {
        public Task<ResponseAPI<List<EsquemaVacunacionReaderDto>>> GetEsquemasVacunacionAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        Task<ResponseAPI<List<EsquemaVacunacionReaderDto>>> GetEsquemaVacunacionByIdAsync(int id);
        Task<ResponseAPI<bool>> CreateEsquemaVacunacionAsync(List<CreateEsquemaVacunacionDto> esquemaVacunacion);
    }
}

