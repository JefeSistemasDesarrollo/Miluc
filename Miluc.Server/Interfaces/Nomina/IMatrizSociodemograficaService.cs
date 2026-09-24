using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IMatrizSociodemograficaService
    {
        public Task<(List<MatrizSociodemograficaReaderDto>data , int CantidadRegistros)> GetMatrizSocioDemograficasAsync(string? filtro=null , int page= 1, int ? cantidad= null, string? correo = null);
        public Task<MatrizSociodemograficaReaderDto> GetMatrizPorEmpleadoIdAsync(int id);

        public Task<bool> updateMatrizAsync(MatrizSocioDemograficaUpdateDto dto);
    }
}
