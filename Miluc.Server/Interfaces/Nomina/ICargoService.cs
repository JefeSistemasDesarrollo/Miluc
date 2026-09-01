using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.Cargos;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ICargoService
    {
        public Task<(List<CargosDto> Data, int TotalRegistros)> GetCargosAsyc(string? filtro = null, int page = 1, int? cantidad = null);
    }
}
