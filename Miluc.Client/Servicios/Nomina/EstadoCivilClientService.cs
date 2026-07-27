using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EstadoCivilDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class EstadoCivilClientService (HttpClient httpClient) : IEstadoCivilClientService
    {
        public async Task<ResponseAPI<List<EstadoCivilReaderDto>>> GetEstadosCivilesAsync()
        {
            try
            {

                var respose = await httpClient.GetFromJsonAsync<ResponseAPI<List<EstadoCivilReaderDto>>>("api/EstadoCivil");

                return respose ?? new ResponseAPI<List<EstadoCivilReaderDto>>()
                {
                    EsCorrecto = false,
                    Mensaje = "No se pudo obtener los estados civiles",
                    Valor = null,
                    CantRegistros = 0
                };  

            }
            catch(Exception ex)
            {

                return new ResponseAPI<List<EstadoCivilReaderDto>>()
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los estados civiles: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };

            }
        }
    }
}
