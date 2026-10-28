using Miluc.Shared.DTOs.Sap.InformesComercial;

namespace Miluc.Server.Interfaces.Sap.Informes.InformeComercial
{
    public interface ISapInformeService
    {
        //Task<Stream> FrecuenciaPorVendedorUltimaFactura(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);
        Task<Stream> FacturasPorVendedorUltimaFactura(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);
        Task<Stream> FacturasPorVendedorUltimaFacturaDetalle(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);
        //historico sin fechas 
        Task<Stream> FacturasPorVendedorCabeceraHistoricoFacturas( int? slpCode = null);
        Task<Stream> FacturasPorVendedorCabeceraHistoricoFacturasDetalle(int? slpCode = null);
        Task<Stream> CanalDeDistribucion(DateTime FechaIncio, DateTime FechaFinal, int? GrDistribucion=null);
        Task<Stream> CarteraVencida(DateTime FechaFinal, string? CardCode = null, int? CodeVendedor = null, int? CodCanal = null);
        //trazabilidad de facturas
        //Task<Stream> TrazabilidadFacturas(DateTime FechaIncio, DateTime FechaFinal, int? CodeVendedor = null);
        Task<Stream> TrazabilidadFacturas(DateTime FechaIncio, DateTime FechaFinal, int? CodeVendedor = null);
    }
}
