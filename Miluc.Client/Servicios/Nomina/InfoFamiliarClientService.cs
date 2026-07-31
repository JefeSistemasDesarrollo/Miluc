using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class InfoFamiliarClientService(HttpClient httpClient) : IInfoFamiliarClientService
    {

        public async Task<ResponseAPI<bool>> CreateFamiliarAsync(List<InfoFamiliarCreateDto> informacionFamiliar)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/InfoFamiliar/CreateInformacionFamiliar", informacionFamiliar);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error API ({response.StatusCode}): {error}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<bool>>();

                return resultado ?? new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "La API no devolvió información.",
                    Valor = false,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = ex.ToString(), // temporal para depuración
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<bool>> DeleteFamiliarAsync(int id)
        {

            try
            {
                var response = await httpClient.DeleteAsync($"api/InfoFamiliar/{id}");
                if (!response.IsSuccessStatusCode)
                {

                    var errorContent = await response.Content.ReadAsStringAsync();

                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al eliminar el familiar: {errorContent}",
                        Valor = false,
                        CantRegistros = 0
                    };

                }

                return new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Mensaje = $"Familiar eliminado correctamente.",
                    Valor = true,
                    CantRegistros = 0
                };

            }

            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al eliminar el familiar: {ex.Message}",
                    Valor = false,
                    Errores = new List<string> { ex.ToString() }, // temporal para depuración
                    CantRegistros = 0
                };
            }
        }



        public async Task<ResponseAPI<List<InfoFamiliarReaderDto>>> GetFamiliarByIdAsync(int idEmpleado)
        {
            try
            {
                //api/InfoFamiliar/{id}
                var resposePeticion = await httpClient.GetAsync($"api/InfoFamiliar/{idEmpleado}");

                if (!resposePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<InfoFamiliarReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener la información familiar: {resposePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await resposePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<InfoFamiliarReaderDto>>>();


                return resultado ?? new ResponseAPI<List<InfoFamiliarReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al deserializar la respuesta.",
                    Valor = null,
                    CantRegistros = 0
                };

            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<InfoFamiliarReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener la información familiar: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };

            }
        }

        public Task<ResponseAPI<List<InfoFamiliarReaderDto>>> GetInfoFamiliaresAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            throw new NotImplementedException();
        }



        public async Task<ResponseAPI<InfoFamiliarReaderDto>> UpdateFamiliarAsync(InfoFamiliarUpdateDto infoFamiliar)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync(
                    $"api/InfoFamiliar/{infoFamiliar.InformacionFamiliarId}",
                    infoFamiliar
                );

                
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<InfoFamiliarReaderDto>>();

                if (result != null)
                {
                    return result;
                }
                //var result = await response.Content
                //    .ReadFromJsonAsync<ResponseAPI<InfoFamiliarReaderDto>>();

                // Si la deserialización falla o viene nula
                return new ResponseAPI<InfoFamiliarReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía o formato inválido del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<InfoFamiliarReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message

                };


            }
        }


    }
        }
  