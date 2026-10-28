using Miluc.Client.Interfaces.SapInterfaces.Oinv;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapOinvClientService (HttpClient _httpclient) : ISapOinvClientService
    {
        public async Task<ResponseAPI<int>> ObtenerPedidoDesdeFacturaObtenerPedidoDesdeFactura(int docEntry)
        {
            try
            {
                var response = await _httpclient.GetAsync($"api/Invoices/pedido-desde-factura/{docEntry}");

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<int>>();

                    return resultado ?? new ResponseAPI<int>
                    {
                        EsCorrecto = false,
                        Mensaje = "No fue posible comunicarse con el servidor."
                    };
                }
                else
                {
                    var erroresResult = await response.Content.ReadFromJsonAsync<ResponseAPI<int>>();
                    return erroresResult ?? new ResponseAPI<int>();
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<int>
                {
                    EsCorrecto = false,
                    CantRegistros = 0,
                    Mensaje = $"Error de red: {ex.Message}"
                };
            }
        }
    }
}
