using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IContratoLaboralService 

    {
        public Task<(List<ContratoLabralreaderDto>data, int CantidadDeRegistros)> GetAllContratoLaboralAsync(string? filtro = null, int page = 1, int? cantidad = null);

        Task<ContratoLabralreaderDto> GetContratoById(int idcontrato);

    }
}
