using Miluc.Shared.DTOs.Sap.ActividadEconomica;
using Miluc.Shared.DTOs.Sap.Cliente.HBT_RESPFISCAL;
using Miluc.Shared.DTOs.Sap.Cliente.RegimeNTributario;
using Miluc.Shared.DTOs.Sap.RegimenFiscal;
using Miluc.Shared.DTOs.Sap.Retenciones;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.LocalizacionSap
{
    public interface ILocalizacionClientSap
    {
       public Task<ResponseAPI<List<RegimenTributarioDto>>> GetAllRegimenTributarioAsync();

        public Task<ResponseAPI<List<TiposDocumentoDto>>> GetAllTiposDocumentoAsync();

        public Task<ResponseAPI<(List<HbtMunicipiosDto> data, int cantidad)>> GetAllMunicipiosAsync(string? buscar = null,int? pagina = null,  int? cantidad = null);

        public Task<ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>> GetAllResponsabilidadesFiscales(string? buscar = null,int? pagina = null,  int? cantidad = null);

        public Task<ResponseAPI<(List<HBT_codigosPostalesDto> data, int cantidad)>> GetAllCodigosPostales(string? buscar = null,int? pagina = null,  int? cantidad = null);

        public Task<ResponseAPI<List<HBT_RegimenFiscalDto>>> GetRegimenFiscal();
        public Task<ResponseAPI<List<RetencionDto>>> GetRetenciones();
        public Task<ResponseAPI<(List<HbtActividadEconomicaDto> data, int cantidad)>> GetActividadEconomica(string? buscar = null, int? pagina = null, int? cantidad = null);
        public Task<ResponseAPI<List<SapResponsabilidadFiscalDto>>> GetResponsabilidadFiscal1(string? buscar = null, int? pagina = null, int? cantidad = null);

    }
}
