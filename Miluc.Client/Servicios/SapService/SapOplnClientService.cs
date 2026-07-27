using Miluc.Client.Interfaces.SapInterfaces.Opln;
using Miluc.Client.Interfaces.SapInterfaces.Oslp;
using Miluc.Shared.DTOs.Sap.Credito;
using Miluc.Shared.DTOs.Sap.Opln;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapOplnClientService(HttpClient _httpClient) : ISapOplnClientService
    {
        public async Task<ResponseAPI<List<OplnReaderDto>>> GetListOplnAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<OplnReaderDto>>>("api/Opln");


                //var result=await response.Content.ReadFromJsonAsync<ResponseAPI<List<DiasCreditoDto>>>();

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<OplnReaderDto>>().ErroresResponse(false, "Error al obtener la lista de precios ", new List<string> { ex.Message });


            }
        }
    }
}
