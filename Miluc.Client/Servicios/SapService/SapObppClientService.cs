using Miluc.Client.Interfaces.SapInterfaces.Rutas;
using Miluc.Shared.DTOs.Sap.Rutas;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapObppClientService(HttpClient http) : ISapObppClientService
    {
        public async Task<ResponseAPI<List<SapObppDto>>> GetObppsAsync()
        {
            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Obpp");


                var response = await http.GetFromJsonAsync<ResponseAPI<List<SapObppDto>>>("api/Obpp");


                return response;

            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
