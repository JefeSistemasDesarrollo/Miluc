using Miluc.Shared.DTOs.Sap.Cliente;
using Miluc.Shared.DTOs.Sap.GrupoDeVenta;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.Cliente
{
    public interface ISapOcrdClientService
    {
        Task<ResponseAPI<List<SapClienteReaderDto>>> GetallClienteAsync(string? buscar = null, int pagina = 1, int? cantidad = null);
        Task<ResponseAPI<List<BusinessPartnerGroupsDto>>> GetGrupoDeVentasAsync();
        Task<ResponseAPI<SapClienteReaderDto>> CreatePedido(SapClienteCreateEditDto createDto);
        Task<ResponseAPI<SapClienteReaderDto>> GetClienteByIdAsync(string cardcode);
        Task<ResponseAPI<SapClienteReaderDto>> ActualizarClienteAsync(string cardCode,SapClienteCreateEditDto sapClienteCreateDto);


        Task<ResponseAPI<bool>> EliminarClienteAsync(string cardCode);


        //ResponseAPI<SapClienteReaderDto>>> ActualizarClienteAsync(string cardCode,[FromBody] SapClienteCreateEditDto sapClienteCreateDto)

    }
}
