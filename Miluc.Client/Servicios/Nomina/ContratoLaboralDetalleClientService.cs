using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class ContratoLaboralDetalleClientService(HttpClient httpClient) : IContratoLaboralDetalleClientService
    {
        public async Task<ResponseAPI<List<ContratoLaboralDetalleDto>>> GetContratoLaboralDetallesAsync()
        {

            try
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/ContratoLaboral/ContratoLaboralDetalle");
            {
                var response = await httpClient.GetAsync($"/api/ContratoLaboral/ContratoLaboralDetalle");
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<ContratoLaboralDetalleDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud: {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };

                }
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<ContratoLaboralDetalleDto>>>();
                if (resultado == null)
                {
                    return new ResponseAPI<List<ContratoLaboralDetalleDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener detalles : respuesta es nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<ContratoLaboralDetalleDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al Obtener los contrato laboral Detalla{ex.Message}",
                    Valor = null,
                    CantRegistros = 0

                };
            }
        }
    }
}






