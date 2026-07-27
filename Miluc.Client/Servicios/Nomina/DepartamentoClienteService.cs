using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.MunicipioDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class DepartamentoClienteService (HttpClient httpClient): IDepartamentosClientService
    {
        public async Task<ResponseAPI<List<MunicipioReaderDto>>> GetMunicipiosAsync()
        {

            try
            {
                var response = await httpClient.GetAsync("api/Departamento/Municipio");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<MunicipioReaderDto>>>();
                    return result;
                }
                else
                {
                    return new ResponseAPI<List<MunicipioReaderDto>>
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
                return new ResponseAPI<List<MunicipioReaderDto>>
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
