using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ParentescoDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class ParentescoClientService(HttpClient httpClient) : IParentescoClient
    {
        public async Task<ResponseAPI<List<ParentescoReaderDto>>> GetAllParentescosAsync()
        {
            try
            {

                var response = await httpClient.GetFromJsonAsync<ResponseAPI<List<ParentescoReaderDto>>>("/api/InfoFamiliar/Parentesco");
               // var response = await httpClient.GetAsync("/api/InfoFamiliar/Parentesco");
                if (response.Valor!=null)
                {
                    //var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<ParentescoReaderDto>>>();
                    return response;
                }
                else
                {
                    return new ResponseAPI<List<ParentescoReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los municipios",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<ParentescoReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los municipios: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
}
