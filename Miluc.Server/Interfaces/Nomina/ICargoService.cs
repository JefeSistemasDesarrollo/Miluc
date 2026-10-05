using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.CargosDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ICargoService
    {
        public Task<(List<CargosReaderDto> Data, int TotalRegistros)> GetCargosAsync(string? filtro = null, int page = 1, int? cantidad = null, string? correo = null);
        public Task<bool> UpsertCargoAsync(UpsertCargos cargoUpdateDto);
        public Task<CargosReaderDto> GetCargoByIdAsync(int cargoId);

        public Task<bool> DeleteCargoAsync(int cargoId);
    }
}
