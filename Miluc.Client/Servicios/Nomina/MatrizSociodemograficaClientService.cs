using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class MatrizSociodemograficaClientService(HttpClient httpClient) : IMatrizSociodemograficaClientService
    {
       public  async Task<ResponseAPI<List<MatrizSociodemograficaReaderDto>>> GetMatrizSociodemograficaAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {

            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/MatrizSociodemografica");
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/MatrizSociodemografica?filtro=&page=1&cantidad=");

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
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<MatrizSociodemograficaReaderDto>>>();
                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<MatrizSociodemograficaReaderDto>>()
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"Error al obtener la matriz sociodemográfica: respuesta nula",
                        CantRegistros = 0
                    };
                }
                return resultado;
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
    }
}


