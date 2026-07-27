using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class ContratoLaboralClientService(HttpClient httpClient) : IContratoLaboralClientService
    {
        public async Task<ResponseAPI<List<ContratoLabralreaderDto>>> GetContratoAsynk(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                // var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/ContratoLaboral/ContratoLaboral?filtro=&page=1&cantidad=");

                var responsePeticion = await httpClient.GetAsync($"api/ContratoLaboral/ContratoLaboral?filtro={textoBusqueda}&page={paginaActual}&cantidad={cantidadPorPagina}"
 );


                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<ContratoLabralreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener Contrato laboral ;{responsePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<ContratoLabralreaderDto>>>();
                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<ContratoLabralreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al deserializar la respueta",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<ContratoLabralreaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error Al obtener Contratos{ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }





                }
            }
        }
    


