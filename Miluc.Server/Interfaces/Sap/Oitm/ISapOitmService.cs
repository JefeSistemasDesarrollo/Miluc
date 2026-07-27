using Miluc.Shared.DTOs.Sap.Articulos;

namespace Miluc.Server.Interfaces.Sap.Oitm
{
    public interface ISapOitmService
    {
        Task<(List<SapOitmDto> Data, int TotalRegistros)> GetListOitmAsync(string? buscar = null, int ? pagina = null, int? cantidad = null);

        //Task<(ArticuloPorListaDePreciosDto precioEspecial, ArticuloPorListaDePreciosDto precioAsignado)> GetListOitmCarcodeItemcodeAsync(string cardcode, string itemCode, int ?cantidad=null);
        Task<ArticuloPorListaDePreciosDto> GetListOitmCarcodeItemcodeAsync(string cardcode, string itemCode, int? cantidad = null);
    }
    
}
