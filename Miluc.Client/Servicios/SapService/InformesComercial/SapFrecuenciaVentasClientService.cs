using Microsoft.JSInterop;
using Miluc.Client.Interfaces.SapInterfaces.Informes;

namespace Miluc.Client.Servicios.SapService.InformesComercial
{
    public class SapFrecuenciaVentasClientService : ISapFrecuenciaVentasClientService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public SapFrecuenciaVentasClientService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<bool> DescargarFrecuanciaVentasService(DateTime fechaInicio, DateTime fechaFinal, int slpCode)
        {
            try
            {
                var url =$"api/Informes/FrecuenciaPorVendedor/Excel" +$"?fechaInicio={fechaInicio:yyyy-MM-dd}" +
               $"&fechaFinal={fechaFinal:yyyy-MM-dd}" +$"&slpCode={slpCode}";

                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var archivo = await response.Content.ReadAsByteArrayAsync();

                await _js.InvokeVoidAsync(
                    "descargarArchivo",
                    $"FrecuenciaVentas_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx",
                    archivo);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

      
    }
}
