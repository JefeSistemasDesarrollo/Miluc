using Miluc.Client.Interfaces.SapInterfaces.Cliente;
using Miluc.Shared.DTOs.Sap.Cliente;
using Miluc.Shared.DTOs.Sap.GrupoDeVenta;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapOcrdClientService(HttpClient _httpclient) : ISapOcrdClientService
    {
        public async Task<ResponseAPI<SapClienteReaderDto>> ActualizarClienteAsync(string cardCode,SapClienteCreateEditDto sapClienteCreateDto)
        {
            var response = await _httpclient.PutAsJsonAsync($"api/Ocrd/{cardCode}",sapClienteCreateDto);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<SapClienteReaderDto>>();

                return resultado!;
            }

            var error = await response.Content.ReadFromJsonAsync<ResponseAPI<SapClienteReaderDto>>();

            return error ?? new ResponseAPI<SapClienteReaderDto>
            {
                EsCorrecto = false,
                Mensaje = $"Error HTTP: {(int)response.StatusCode}"
            };
        }

        public async Task<ResponseAPI<SapClienteReaderDto>> CreatePedido(SapClienteCreateEditDto sapClienteCreateDto)
        {
            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7222/api/Ocrd");
              //  var response = await _httpclient.PostAsJsonAsync("api/Ocrd", sapClienteCreateDto);

                var response = await _httpclient.PostAsJsonAsync("api/Ocrd", sapClienteCreateDto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<SapClienteReaderDto>>();
                    return result ?? new ResponseAPI<SapClienteReaderDto>();
                }
                else
                {
                    var erroresResult = await response.Content.ReadFromJsonAsync<ResponseAPI<SapClienteReaderDto>>();
                    return erroresResult ?? new ResponseAPI<SapClienteReaderDto>();
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                };
            }
        }

        public async Task<ResponseAPI<bool>> EliminarClienteAsync(string cardCode)
        {

            try
            {
                var response = await _httpclient.DeleteAsync($"api/Ocrd/{cardCode}");

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                    return resultado ?? new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "No fue posible comunicarse con el servidor."
                    };
                }
                else
                {
                    var erroresResult = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                    return erroresResult ?? new ResponseAPI<bool>();
                }
            }
            catch(Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    CantRegistros = 0,
                    Mensaje = $"Error de red: {ex.Message}"
                };
            }
        }
        public async Task<ResponseAPI<List<SapClienteReaderDto>>> GetallClienteAsync(string? buscar = null, int pagina = 1, int? cantidad = null, int? codVendedorSAP = null)
        {
            try
            {
                ResponseAPI<List<SapClienteReaderDto>> responseClient = new();
                var url = $"api/Ocrd?buscar={buscar}&pagina={pagina}&cantidad={cantidad}&codVendedorSAP={codVendedorSAP}";
                var response = await _httpclient.GetFromJsonAsync<ResponseAPI<List<SapClienteReaderDto>>>(url);

                if (response != null)
                {
                    //devolve,os la lista 
                    responseClient.Valor = response.Valor;
                    responseClient.EsCorrecto = response.EsCorrecto;
                    responseClient.Errores = response.Errores;
                    responseClient.CantRegistros = response.CantRegistros;
                    return responseClient;
                }
                else
                {
                    return new ResponseAPI<List<SapClienteReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = new List<SapClienteReaderDto>(),
                        CantRegistros = 0,
                        Mensaje = response?.Mensaje ?? "Error al obtener datos"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<SapClienteReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<SapClienteReaderDto>(),
                    CantRegistros = 0,
                    Mensaje = $"Error de red: {ex.Message}"
                };

            }
        }

        public async Task<ResponseAPI<SapClienteReaderDto>> GetClienteByIdAsync(string cardcode)
        {
            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Ocrd/{cardcode}");

                ResponseAPI<SapClienteReaderDto> response =new ();
                var result = await _httpclient.GetFromJsonAsync<ResponseAPI<SapClienteReaderDto>>($"api/Ocrd/{cardcode}");

                if (result.Valor != null)
                {
                    response.Valor = result.Valor;
                    response.EsCorrecto = result.EsCorrecto;
                    response.Mensaje=result.Mensaje;
                    response.CantRegistros = result.CantRegistros;

                    return response.SuccessResponse(response.EsCorrecto, response.Mensaje, response.Valor, response.CantRegistros);
                }
                else
                {
                    return response.ErroresResponse(result.EsCorrecto, result.Mensaje,result.Errores);
                }

            }
            catch (Exception ex) 
            {

                List<string>Errores = new List<string>();
                return new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = ex.Message,
                };
            
            
            }
        }

        public async Task<ResponseAPI<List<BusinessPartnerGroupsDto>>> GetGrupoDeVentasAsync()
        {
            try
            {

                var response = await _httpclient.GetFromJsonAsync<ResponseAPI<List<BusinessPartnerGroupsDto>>>("api/BusinessPartnerGroups");

                if (response == null)
                {
                    return new ResponseAPI<List<BusinessPartnerGroupsDto>>
                    {
                        EsCorrecto = false,
                        Valor = new List<BusinessPartnerGroupsDto>(),
                        CantRegistros = 0,
                        Mensaje = "Error al obtener datos"
                    };

                }
                else
                {
                    return response;

                }

            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<BusinessPartnerGroupsDto>>
                {
            EsCorrecto = false,
                    Valor = new List<BusinessPartnerGroupsDto>(),
                    CantRegistros = 0,
                    Mensaje = $"Error de red: {ex.Message}"
                };


}
        }
    }
}
