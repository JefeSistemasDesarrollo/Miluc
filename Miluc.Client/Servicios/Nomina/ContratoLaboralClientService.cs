using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class ContratoLaboralClientService(HttpClient _httpClient) : IContratoLaboralClientService
    {
        public async Task<ResponseAPI<List<ContratoLabralreaderDto>>> GetContratoLaboralAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                // var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/ContratoLaboral/ContratoLaboral?filtro=&page=&cantidad=");

                var responsePeticion = await _httpClient.GetAsync($"api/ContratoLaboral/ContratoLaboral?filtro={textoBusqueda}&page={paginaActual}&cantidad={cantidadPorPagina}"
 );

                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<ContratoLabralreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener contratos: {responsePeticion.ReasonPhrase}",
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
                        Mensaje = "Error al obtener  contratos: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }


                return resultado;




            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<ContratoLabralreaderDto>> { EsCorrecto = false, Mensaje = $"Error al obtener  contratos: {ex.Message}", Valor = null, CantRegistros = 0 };
            }
        }
    






                

        public async Task<ResponseAPI<ContratoLabralreaderDto>> GetContratoLaboralByIdAsync(int id)
        {
            try
            {

                ResponseAPI<ContratoLabralreaderDto> responseAPI = new ResponseAPI<ContratoLabralreaderDto>();

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<ContratoLabralreaderDto>>($"/api/ContratoLaboral/{id}");

                if (response.Valor != null)
                {
                    responseAPI.Valor = response.Valor;
                    responseAPI.Mensaje = response.Mensaje;
                    responseAPI.EsCorrecto = response.EsCorrecto;


                }


                return responseAPI;


            }
            catch (Exception ex)
            {

                return new ResponseAPI<ContratoLabralreaderDto>
                {
                    Errores = new List<string> { $"Error {ex}" },
                    Mensaje = ex.Message,
                    EsCorrecto = false


                };

            }
        }

        public async Task<ResponseAPI<ContratoLabralreaderDto>> UpdateContratoLaboralAsync(UpdateContratoLaboralDto contrato)
        {
            try
            {
               
                var response = await _httpClient.PutAsJsonAsync(
                    $"/api/ContratoLaboral/{contrato.ContratoLaboralId}",
                    contrato
                );

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<ContratoLabralreaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = await response.Content.ReadAsStringAsync()
                    };
                }

                var result = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<ContratoLabralreaderDto>>();

                return result ?? new ResponseAPI<ContratoLabralreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía en el servidor"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<ContratoLabralreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
    }
    }
        
    


