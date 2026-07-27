using Miluc.Client.Interfaces.SapInterfaces.Oslp;
using Miluc.Shared.DTOs.Sap.Vendedor;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapOslpService(HttpClient _httpclient) : ISapOslpService
    {
        public async Task<ResponseAPI<List<SapVendedorReaderDto>>> GetListVendedoresAsync()
        {
            try
            {
                var response = await _httpclient.GetAsync("api/Oslp");
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<SapVendedorReaderDto>>>();

             

                return result;

            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<SapVendedorReaderDto>>().ErroresResponse(false, "Error al obtener la lista de vendedores", new List<string> { ex.Message });

            }
        }
    }
}
