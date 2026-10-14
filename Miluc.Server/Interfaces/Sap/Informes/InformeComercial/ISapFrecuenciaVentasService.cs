using Miluc.Shared.DTOs.Sap.InformesComercial;

namespace Miluc.Server.Interfaces.Sap.Informes.InformeComercial
{
    public interface ISapFrecuenciaVentasService
    {
        //Task<Stream> FrecuenciaPorVendedorUltimaFactura(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);
        Task<Stream> FrecuenciaPorVendedorUltimaFactura(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);
        Task<Stream> FrecuenciaPorVendedorUltimaFacturaDetalle(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);

        //historico sin fechas 
        Task<Stream> FrecuenciaPorVendedorCabeceraHistoricoFacturas( int? slpCode = null);
        Task<Stream> FrecuenciaPorVendedorCabeceraHistoricoFacturasDetalle(int? slpCode = null);





        Task<Stream> CanalDeDistribucion(DateTime FechaIncio, DateTime FechaFinal, int? GrDistribucion=null);
        Task<Stream> CarteraVencida(DateTime FechaFinal, string? CardCode = null, int? CodeVendedor = null, int? CodCanal = null);
    }
}
