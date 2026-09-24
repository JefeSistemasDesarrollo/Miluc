using Miluc.Shared.DTOs.Sap.InformesComercial;

namespace Miluc.Server.Interfaces.Sap.Informes.InformeComercial
{
    public interface ISapFrecuenciaVentasService
    {
         Task<Stream> FrecuenciaPorVendedorUltimaFactura(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);

    }
}
