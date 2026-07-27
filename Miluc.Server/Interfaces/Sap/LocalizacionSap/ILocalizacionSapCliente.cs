using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.ActividadEconomica;
using Miluc.Shared.DTOs.Sap.Cliente.HBT_RESPFISCAL;
using Miluc.Shared.DTOs.Sap.Cliente.RegimeNTributario;
using Miluc.Shared.DTOs.Sap.RegimenFiscal;
using Miluc.Shared.DTOs.Sap.Retenciones;

namespace Miluc.Server.Interfaces.Sap.LocalizacionSap
{
    public interface ILocalizacionSapCliente
    {
        Task<List<RegimenTributarioDto>> GetAllRegimenTributarioAsync();

        Task<List<TiposDocumentoDto>> GetAllTiposDocumentoAsync();

        Task<(List<HbtMunicipiosDto> data, int cantidad)> GetAllMunicipiosAsync(string? buscar = null, int ? pagina = null, int? cantidad = null);

        Task<List<OkiResponsabilidadesFiscalesDto>> GetAllResponsabilidadesFiscalesAsync(string? buscar = null, int ?pagina = null, int? cantidad = null);

       // Task<List<HBT_codigosPostalesDto>> GetAllCodigosPostalesAsync(string? buscar = null, int pagina = 1, int? cantidad = null);
        Task<(List<HBT_codigosPostalesDto>data, int cantidadRegistros)> GetAllCodigosPostalesAsync(string? buscar = null, int ?pagina = null, int? cantidad = null);

        Task<List<HBT_RegimenFiscalDto>> GetAllRegimenFiscal();

        Task<List<RetencionDto>> getAllRetenciones();
        Task<(List<HbtActividadEconomicaDto> data, int cantidad)> GetActividadEconomica(string? buscar = null, int ?pagina = null, int? cantidad = null);
        Task<List<SapResponsabilidadFiscalDto>> GetResponsabilidadFiscal1(string? buscar = null, int ?pagina = null, int? cantidad = null);

    }
}
