namespace Miluc.Client.Interfaces.SapInterfaces.Informes
{
    public interface ISapFrecuenciaVentasClientService
    {
        Task<bool> DescargarFrecuanciaVentasService(DateTime fechaInicio, DateTime fechaFinal, int slpCode);
    }
}
