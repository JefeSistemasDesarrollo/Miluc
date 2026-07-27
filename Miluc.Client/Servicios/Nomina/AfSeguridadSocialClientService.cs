using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class AfSeguridadSocialClientService(HttpClient _httpClient) : IAfSeguridadSocialClientService
    {
        public async Task<ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>> GetAfSeguridadSocialAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                // var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/AfiliacionSeguridadSocial/AfiliacionSeguridadSocial?filtro=&page=1&cantidad=");
                var response = await _httpClient.GetAsync($"api/AfiliacionSeguridadSocial/AfiliacionSeguridadSocial?filtro={textoBusqueda}&page={paginaActual}&cantidad={cantidadPorPagina}");
                if (!response.IsSuccessStatusCode)
                { // Manejar el error de la solicitud
                    return new ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud: {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>>();
                if (resultado == null)
                {
                    return new ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los Empleados:  respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                return resultado;

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los Empleados: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0

                };
            }



        }




        public async Task<ResponseAPI<AfiliacionSeguridadSocialreaderDto>> UpdateAfiliacionAsync(UpdateAFiliacionDto afiliacion)
        {
            try
            {
                //  CORRECCIÓN: Se añade "/" antes de {afiliacion.AfiliacionId}
                var response = await _httpClient.PutAsJsonAsync(
                    $"/api/AfiliacionSeguridadSocial/{afiliacion.AfiliacionId}",
                    afiliacion
                );

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = await response.Content.ReadAsStringAsync()
                    };
                }

                var result = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<AfiliacionSeguridadSocialreaderDto>>();

                return result ?? new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía en el servidor"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
        public async Task<ResponseAPI<AfiliacionSeguridadSocialreaderDto>> GetAfiliacionSeguridadSocialByIdAsync(int idEmpleado)
        {

            try
            {

                ResponseAPI<AfiliacionSeguridadSocialreaderDto> responseAPI = new ResponseAPI<AfiliacionSeguridadSocialreaderDto>();

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<AfiliacionSeguridadSocialreaderDto>>($"/api/AfiliacionSeguridadSocial/{idEmpleado}");

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

                return new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                {
                    Errores = new List<string> { $"Error {ex}" },
                    Mensaje = ex.Message,
                    EsCorrecto = false


                };

            }

        }
    }
        }


