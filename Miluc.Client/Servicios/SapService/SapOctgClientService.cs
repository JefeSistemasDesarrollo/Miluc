using Miluc.Client.Interfaces.SapInterfaces.Octg;
using Miluc.Shared.DTOs.Sap.Credito;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapOctgClientService(HttpClient _httpClient) : ISapOctgClientService
    {
        public async Task<ResponseAPI<List<DiasCreditoDto>>> GetAllOctgAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync < ResponseAPI<List<DiasCreditoDto>>>("api/Octg");

                
                //var result=await response.Content.ReadFromJsonAsync<ResponseAPI<List<DiasCreditoDto>>>();

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<DiasCreditoDto>>().ErroresResponse(false, "Error al obtener los dias de credito", new List<string> { ex.Message });


            }
        }
    }
}
