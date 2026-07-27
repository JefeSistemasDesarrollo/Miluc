using Miluc.Shared.DTOs.Nomina.EsquemaVacunbacionDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IEsquemaVacunacionService  
    {
        Task<(List<EsquemaVacunacionReaderDto>data, int CantidadRegitros)> GetEsquemasVacunacionAsync(string? filtro = null, int page = 1, int? cantidad = null);
        Task<EsquemaVacunacionReaderDto> GetEsquemaVacunacionByIdAsync(int id);
        Task<EsquemaVacunacionReaderDto> CreateEsquemaVacunacionAsync(EsquemaVacunacionCreateDto esquemaVacunacion);

    }
}
