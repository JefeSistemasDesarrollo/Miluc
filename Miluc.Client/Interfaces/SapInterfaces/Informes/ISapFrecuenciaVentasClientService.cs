namespace Miluc.Client.Interfaces.SapInterfaces.Informes
{
    public interface ISapFrecuenciaVentasClientService
    {
        Task<bool> DescargarFrecuanciaVentasService(DateTime fechaInicio, DateTime fechaFinal, int slpCode);
        // Task<bool> DescargarCarteraVencidaService(DateTime ?fechaVencimiento=null, string? cardCode = null, int? codeVendedor = null, int? codCanal = null);

        Task<bool> CarteraVencidaExcel(DateTime fechaFinal, string? cardCode = null, int? codeVendedor = null, int? codCanal = null);


    }
}
