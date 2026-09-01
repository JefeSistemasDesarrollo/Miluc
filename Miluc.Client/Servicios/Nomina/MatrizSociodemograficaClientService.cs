using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;
using System.Text.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class MatrizSociodemograficaClientService(HttpClient httpClient) : IMatrizSociodemograficaClientService


    {
        public async Task<ResponseAPI<List<MatrizSociodemograficaReaderDto>>> GetMatrizSociodemograficaAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                var responsePeticion = await httpClient.GetAsync($"api/MatrizSocioDemografica?filtro={textoBusqueda}&page={paginaActual}&cantidad={cantidadPorPagina}");

                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<MatrizSociodemograficaReaderDto>>()
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"Error al obtener la matriz sociodemográfica: {responsePeticion.ReasonPhrase}",
                        CantRegistros = 0
                    };
                }

                // Configuración para ignorar diferencias entre Mayúsculas/Minúsculas en nombres de propiedades
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<MatrizSociodemograficaReaderDto>>>(options);

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<MatrizSociodemograficaReaderDto>>()
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "Error al obtener la matriz sociodemográfica: respuesta nula",
                        CantRegistros = 0
                    };
                }

                return resultado;
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                // Captura específica para errores de mapeo/deserialización JSON
                return new ResponseAPI<List<MatrizSociodemograficaReaderDto>>()
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error de formato JSON: {jsonEx.Message}",
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<MatrizSociodemograficaReaderDto>>()
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error al obtener la matriz sociodemográfica: {ex.Message}",
                    CantRegistros = 0
                };
            }
        }
        public async Task<ResponseAPI<MatrizSociodemograficaReaderDto>>
        GetMatrizPorEmpleadoIdAsync(int empleadoId)
        {
            try
            {
                var response =
                    await httpClient.GetFromJsonAsync<ResponseAPI<MatrizSociodemograficaReaderDto>>($"api/MatrizSocioDemografica/{empleadoId}");

                return response ?? new ResponseAPI<MatrizSociodemograficaReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se obtuvo respuesta del servidor"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<MatrizSociodemograficaReaderDto>
                {
                    Errores = new List<string> { ex.Message },
                    Mensaje = ex.Message,
                    EsCorrecto = false
                };
            }
        }


        public async Task<ResponseAPI<MatrizSociodemograficaReaderDto>> updateMatrizAsync(MatrizSocioDemograficaUpdateDto matriz)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/MatrizSocioDemografica/{matriz.MatrizSociodemograficaId}",matriz);

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<MatrizSociodemograficaReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = await response.Content.ReadAsStringAsync()
                    };
                }

                var result = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<MatrizSociodemograficaReaderDto>>();

                return result ?? new ResponseAPI<MatrizSociodemograficaReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía en el servidor"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<MatrizSociodemograficaReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
    }
}