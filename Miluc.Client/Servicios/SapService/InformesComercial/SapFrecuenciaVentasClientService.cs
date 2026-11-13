using Microsoft.JSInterop;
using Miluc.Client.Interfaces.SapInterfaces.Informes;

namespace Miluc.Client.Servicios.SapService.InformesComercial
{
    public class SapFrecuenciaVentasClientService : ISapVentasClientService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public SapFrecuenciaVentasClientService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

    
        public async Task<bool> CanalDeDistribucionAsync(DateTime FechaIncio, DateTime FechaFinal, int? GrDistribucion = null)
        {
            try
            {
                var url = $"api/Informes/CanalDeDistribucion/Excel" + $"?fechaInicio={FechaIncio:yyyy-MM-dd}" +$"&FechaFinal={FechaFinal:yyyy-MM-dd}" + $"&GrDistribucion={GrDistribucion}";

                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var archivo = await response.Content.ReadAsByteArrayAsync();

                await _js.InvokeVoidAsync(
                    "descargarArchivo",
                    $"canalaDedis{FechaIncio:yyyyMMdd}_{FechaFinal:yyyyMMdd}.xlsx",
                    archivo);

                return true;


            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el informe de canal de distribución {ex.Message}");
            }
        }

        public async Task<bool> CarteraVencidaExcel(string? cardCode = null, int? codeVendedor = null, int? codCanal = null)
        {
            try
            {
                var url = $"api/Informes/CarteraVencida/Excel?cardCode={cardCode} &codeVendedor={codeVendedor} &codCanal={codCanal} ";
                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                var archivo = await response.Content.ReadAsByteArrayAsync();

                await _js.InvokeVoidAsync(
                    "descargarArchivo",
                    $"CarteraVencida.xlsx",
                    archivo);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el informe de cartera vencida {ex.Message}");
            }
        }
        public async Task<bool> DescargarFacturasVentasService(DateTime fechaInicio, DateTime fechaFinal, int ?slpCode=null, int? tipoInforme = null)
        {
            try
            {
                var url = $"api/Informes/FacturasPorVendedor/Excel" + $"?fechaInicio={fechaInicio:yyyy-MM-dd}" +
               $"&fechaFinal={fechaFinal:yyyy-MM-dd}" + $"&slpCode={slpCode}" + $"&tipoInforme={tipoInforme}";

                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var archivo = await response.Content.ReadAsByteArrayAsync();

                if (archivo == null || archivo.Length == 0)
                {
                    return false;
                }

                if (tipoInforme == 1)
                {
                    await _js.InvokeVoidAsync(
                        "descargarArchivo",
                        $"FacturasVentasCabecera_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx",
                        archivo);
                }
                else if (tipoInforme == 2)
                {
                    await _js.InvokeVoidAsync(
                        "descargarArchivo",
                        $"FacturasVentasDetalle_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx",
                        archivo);
                }
                else if (tipoInforme == 3)
                {
                    await _js.InvokeVoidAsync(
                        "descargarArchivo",
                        $"FacturasVentasHistorico_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx",
                        archivo);

                }
                else if (tipoInforme == 4)
                {
                    await _js.InvokeVoidAsync(
                        "descargarArchivo",
                        $"FacturasVentasHistoricoDetalle_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx",
                        archivo);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception($"Error al generar el informe de ventas de venta {ex.Message}");
            }
        }



        public async Task<bool> DescargarTrazabilidad(DateTime fechaInicio, DateTime fechaFinal, int ?slpCode=null)
        {
            try
            {

                ///         api/Informes/CarteraVencida/Excel?fechaFinal=2026-10-02&cardCode=&codeVendedor=&codCanal=
                //  var url = $"api/Informes/CarteraVencida/Excel?fechaFinal={fechaFinal:yyyy-MM-dd}&cardCode={cardCode}&codeVendedor={codeVendedor}&codCanal={codCanal}";
                var url = $"api/Informes/Trazabilidad/Excel" + $"?FechaIncio={fechaInicio:yyyy-MM-dd}" + $"&FechaFinal={fechaFinal:yyyy-MM-dd}" + $"&CodeVendedor={slpCode}" ;


              
                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var archivo = await response.Content.ReadAsByteArrayAsync();

                await _js.InvokeVoidAsync(
                    "descargarArchivo",
                    $"CarteraVencida_{fechaFinal:yyyyMMdd}.xlsx",
                    archivo);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el informe de cartera vencida {ex.Message}");
            }
        }

        public async Task<bool> DescargarVentasPorVendedorExcel(DateTime fechaInicio, DateTime fechaFinal, int ?slpCode = null, int ?tipoInforme = null)
        {
            try
            {
                var url = $"api/Informes/VentasPorVendedor/Excel" + $"?fechaInicio={fechaInicio:yyyy-MM-dd}" +$"&fechaFinal={fechaFinal:yyyy-MM-dd}" + $"&slpCode={slpCode}" + $"&tipoInforme={tipoInforme}";            
                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }


                var archivo = await response.Content.ReadAsByteArrayAsync();

                if (archivo == null || archivo.Length == 0)
                {
                    return false;
                }

                if(tipoInforme == 1)
                {
                    await _js.InvokeVoidAsync(
                          "descargarArchivo",
                          $"VenDetalle_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx",
                          archivo);
                }
                else if (tipoInforme == 2)
                {
                    await _js.InvokeVoidAsync(
                        "descargarArchivo",
                        $"VenCons_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx",
                        archivo);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el informe de ventas por vendedor {ex.Message}");
            }
        }


    }
}
