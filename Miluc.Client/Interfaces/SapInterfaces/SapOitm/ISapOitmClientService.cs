using Miluc.Shared.DTOs.Sap.Articulos;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.SapOitm
{
    public interface ISapOitmClientService
    {
        Task<ResponseAPI<List<SapOitmDto>>> GetListOitmAsync(string? buscar = null, int ?pagina = null, int? cantidad = null);

        Task<ResponseAPI<ArticuloPorListaDePreciosDto>> GetOitmCarcodeItemcodeAsync(string cardcode, string itemCode, int? cantidad = null);
    }
}
