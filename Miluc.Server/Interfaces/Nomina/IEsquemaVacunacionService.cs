using Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto;




namespace Miluc.Server.Interfaces.Nomina
{
    public interface IEsquemaVacunacionService  
    {
       public Task<(List<EsquemaVacunacionReaderDto> Data, int CantidadRegistros)> GetEsquemasVacunacionAsync(string? filtro = null, int page = 1, int? cantidad = null);
       public  Task<List<EsquemaVacunacionReaderDto>> GetEsquemaVacunacionByIdAsync(int id);
       public  Task<bool> CreateEsquemaVacunacionAsync(List<CreateEsquemaVacunacionDto> esquemaVacunacion);

    }
}
