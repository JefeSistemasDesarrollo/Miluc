using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.NivelAcademico;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class NivelAcademicoClientService(HttpClient _httpClient) : INivelAcademicoClientService
    {
        public async Task<ResponseAPI<List<NivelAcademicoReaderDto>>> GetNivelAcademicoAsync()
        {
            try
            { var response = await _httpClient.GetAsync("/api/TablasMatriz/NivelAcademico");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<NivelAcademicoReaderDto>>>();
                    return result; 
                }
                else
                {
                    return new ResponseAPI<List<NivelAcademicoReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener el nivel académico.",
                        Errores = new List<string> { response.ReasonPhrase },
                        CantRegistros = 0
                    };
                }
                
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<NivelAcademicoReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al obtener el nivel académico.",
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };
            }
        }
    }
}

        
    

