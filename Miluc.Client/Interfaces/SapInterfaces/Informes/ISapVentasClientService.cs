namespace Miluc.Client.Interfaces.SapInterfaces.Informes
{
    public interface ISapVentasClientService
    {
        Task<bool> DescargarFacturasVentasService(DateTime fechaInicio, DateTime fechaFinal, int slpCode, int tipoInforme);
        Task<bool> CanalDeDistribucionAsync(DateTime FechaIncio, DateTime FechaFinal, int? GrDistribucion = null);
        Task<bool> CarteraVencidaExcel(DateTime fechaFinal, string? cardCode = null, int? codeVendedor = null, int? codCanal = null);
        Task<bool> DescargarTrazabilidad(DateTime fechaInicio, DateTime fechaFinal, int slpCode);
    }
}
