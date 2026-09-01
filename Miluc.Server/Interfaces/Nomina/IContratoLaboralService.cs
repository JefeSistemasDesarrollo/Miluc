using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;


namespace Miluc.Server.Interfaces.Nomina
{
    public interface IContratoLaboralService 

    {
        public Task<(List<ContratoLaboralreaderDto>data, int CantidadDeRegistros)> GetContratoLaboralAsync(string? filtro = null, int ? page = null, int? cantidad = null);

        public Task<List<ContratoLaboralreaderDto>> GetContratosPorEmpleadoAsync(int empleadoId);
        public Task<bool> CreateContratoLaboralAsync(CreateContratoLaboralDto contrato);
        public Task<ContratoLaboralreaderDto> UpdateContratoAsync(UpdateContratoDto  update);
        public Task<bool> InhabilitarContratoAsync(InhabilitarContratoDto dto);
        public Task<ContratoLaboralreaderDto?> GetContratoByIdAsync(int contratoLaboralId);

    }
}
