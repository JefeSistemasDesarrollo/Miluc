using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IContratoLaboralService 

    {
        public Task<(List<ContratoLabralreaderDto>data, int CantidadDeRegistros)> GetContratoLaboralAsync(string? filtro = null, int ? page = null, int? cantidad = null);

       public Task<ContratoLabralreaderDto> GetContratoById(int idcontrato);
        public Task<bool> UpsertContratoLaboralAsync(UpdateContratoLaboralDto dto);

    }
}
