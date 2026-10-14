namespace Miluc.Client.Interfaces.SapInterfaces.Informes
{
    public interface ISapFrecuenciaVentasClientService
    {
        Task<bool> DescargarFrecuanciaVentasService(DateTime fechaInicio, DateTime fechaFinal, int slpCode, int tipoInforme);

        Task<bool> CanalDeDistribucionAsync(DateTime FechaIncio, DateTime FechaFinal, int? GrDistribucion = null);
        //Task<bool> DescargarFrecuanciaVentasDetalleService(DateTime fechaInicio, DateTime fechaFinal, int slpCode);
        // Task<bool> DescargarCarteraVencidaService(DateTime ?fechaVencimiento=null, string? cardCode = null, int? codeVendedor = null, int? codCanal = null);


        //Task<Stream> FrecuenciaPorVendedorCabeceraFacturas(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);
        //Task<Stream> FrecuenciaPorVendedorFacturaDetalle(DateTime FechaInicio, DateTime FechaFinal, int? slpCode = null);

        ////historico sin fechas 
        //Task<Stream> FrecuenciaPorVendedorCabeceraHistoricoFacturas(int? slpCode = null);
        //Task<Stream> FrecuenciaPorVendedorCabeceraHistoricoFacturasDetalle(int? slpCode = null);




        Task<bool> CarteraVencidaExcel(DateTime fechaFinal, string? cardCode = null, int? codeVendedor = null, int? codCanal = null);


    }
}
