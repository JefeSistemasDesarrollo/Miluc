using Miluc.Shared.DTOs.Nomina.PaisDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IPaisService
    {
        public Task<(List<PaisReaderDto> data, int CantidadRegistros)> GetPaisAsync(string? filtro=null, int  page = 1, int ? cantidad = null);   
    }
}
