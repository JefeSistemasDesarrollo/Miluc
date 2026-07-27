using Miluc.Client.Interfaces.SapInterfaces.SapOitm;
using Miluc.Shared.DTOs.Sap.Articulos;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapOitmClientService(HttpClient _httpClient) :ISapOitmClientService
    {
        public async Task<ResponseAPI<List<SapOitmDto>>> GetListOitmAsync(string? buscar = null, int ? pagina = null, int? cantidad = null)
        {
            try
            {
                ResponseAPI<List<SapOitmDto>> responseApiOitm = new();
                //var request = new HttpRequestMessage(HttpMethod.Get,
                //"https://192.168.1.10:7222/api/Oitm?buscar=&pagina=1&cantidad=");


                var url = $"api/Oitm?buscar={buscar}&pagina={pagina}&cantidad={cantidad}";

                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<SapOitmDto>>>(url);

                if (response != null)
                {
                    responseApiOitm.Valor = response.Valor;
                    responseApiOitm.EsCorrecto = response.EsCorrecto;
                    responseApiOitm.Mensaje = response.Mensaje;
                    responseApiOitm.Errores = response.Errores;
                    responseApiOitm.CantRegistros = response.CantRegistros;

                    return responseApiOitm;
                }
                else
                {
                    return new ResponseAPI<List<SapOitmDto>>
                    {
                        EsCorrecto = false,
                        Valor = new List<SapOitmDto>(),
                        CantRegistros = 0,
                        Mensaje = response?.Mensaje ?? "Error al ontener los articulos"
                    };


                }

            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<SapOitmDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<SapOitmDto>(),
                    CantRegistros = 0,
                    Mensaje = $"Error al obtener los articulos: {ex.Message}"
                };

            }
        }


        public async Task<ResponseAPI<ArticuloPorListaDePreciosDto>>
    GetOitmCarcodeItemcodeAsync(string cardcode, string itemCode, int? cantidad = null)
        {
            try
            {
                var url = $"api/Oitm/oitmCardcodeItemCode?" +
                          $"cardcode={Uri.EscapeDataString(cardcode)}&" +
                          $"itemcode={Uri.EscapeDataString(itemCode)}&" +
                          $"cantidad={cantidad}";

                var response = await _httpClient.GetFromJsonAsync<
                    ResponseAPI<ArticuloPorListaDePreciosDto>>(url);

                return response ?? new ResponseAPI<ArticuloPorListaDePreciosDto>
                {
                    EsCorrecto = false,
                    Valor = (new ArticuloPorListaDePreciosDto()),
                    CantRegistros = 0,
                    Mensaje = "Error al obtener los artículos (respuesta nula)"
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<ArticuloPorListaDePreciosDto>
                {
                    EsCorrecto = false,
                    Valor = (new ArticuloPorListaDePreciosDto()),
                    CantRegistros = 0,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }
    }
}
