using Miluc.Shared.DTOs.Sap.Rutas;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.Rutas
{
    public interface ISapObppClientService
    {
        Task<ResponseAPI<List<SapObppDto>>> GetObppsAsync();


    }
}
