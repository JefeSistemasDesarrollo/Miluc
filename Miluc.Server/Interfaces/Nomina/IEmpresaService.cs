using Miluc.Shared.DTOs.Nomina.EmpresaDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IEmpresaService
    {
        public Task<List<EmpresaReaderDto>> GetEmpresasAsync(string? filtro = null, int page = 1, int? cantidad = null, string? correo = null);
        public Task<EmpresaReaderDto> GetEmpresaByIdAsync(int empresaId);
        public Task<bool> UpsertEmpresaAsync(UpsertEmpresaDto empresaUpdateDto);
        public Task<bool> DeleteEmpresaAsync(int id);
    }
}
