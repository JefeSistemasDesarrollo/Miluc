using Miluc.Client.Interfaces.SapInterfaces.SapOrdr;
using Miluc.Shared.DTOs.Sap.Pedidos;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;
using System.Text;

namespace Miluc.Client.Servicios.SapService
{
    public class SapOrdrService(HttpClient http) : ISapOrdrService
    {
        public async Task<ResponseAPI<OrdersReaderDto>> CreatePedidoAsyc(PedidoCreateDto pedidoCreateDto)
        {
            try
            {
                //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7222/api/Orders");
                var response = await http.PostAsJsonAsync("api/Orders", pedidoCreateDto);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<OrdersReaderDto>>();
                    return result ?? new ResponseAPI<OrdersReaderDto>();
                }
                else
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ResponseAPI<OrdersReaderDto>>();
                    return errorResult ?? new ResponseAPI<OrdersReaderDto>();
                }
            }
            catch (Exception ex)

            {
                return new ResponseAPI<OrdersReaderDto> { EsCorrecto = false, Mensaje = ex.Message };
            }
        }

        public async Task<ResponseAPI<List<OrdersReaderDto>>> ListarPedidosAsync(string? buscar = null, int? pagina = null, int? cantidad = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, int? codVendedorSAP = null, char? DocStatus = null, char? CANCELED = null, char? Printed = null)
        {
            try
            {

                ResponseAPI<List<OrdersReaderDto>> responseApiPeidos = new ResponseAPI<List<OrdersReaderDto>>();
                //url
                var url = $"api/Orders?buscar={buscar}&pagina={pagina}&cantidad={cantidad}";
                //url con las fechas 
                if (fechaInicio.HasValue)
                {
                    url += $"&fechaInicio={fechaInicio.Value:yyyy-MM-dd}";

                }
                if (fechaFin.HasValue)
                {
                    url += $"&fechaFin={fechaFin.Value:yyyy-MM-dd}";

                }
                if (codVendedorSAP.HasValue)
                {
                    url += $"&codVendedorSAP={codVendedorSAP.Value}";
                }
                if (DocStatus!=null)
                {
                    url +=$"&DocStatus={DocStatus}";
                }
                if (CANCELED!=null)
                {
                    url += $"&CANCELED={CANCELED}";
                }
                if (Printed!=null)
                {
                    url += $"&Printed={Printed}";
                }

                var response = await http.GetFromJsonAsync<ResponseAPI<List<OrdersReaderDto>>>(url);


                if (response.Valor != null)
                {

                    responseApiPeidos.Valor = response.Valor;
                    responseApiPeidos.EsCorrecto = response.EsCorrecto;
                    responseApiPeidos.Mensaje = response.Mensaje;
                    responseApiPeidos.Errores = response.Errores;
                    responseApiPeidos.CantRegistros = response.CantRegistros;

                    return responseApiPeidos;

                }
                else
                {
                    return new ResponseAPI<List<OrdersReaderDto>> { EsCorrecto = false, Mensaje = "No se encontraron pedidos." };
                }


            }
            catch (Exception ex)
            {

                return new ResponseAPI<List<OrdersReaderDto>> { EsCorrecto = false, Mensaje = ex.Message };

            }
        }

        public async Task<ResponseAPI<OrdersReaderDto>> GetOrderByIdAsync(int DocEntry)
        {
            try
           
            {
               ResponseAPI<OrdersReaderDto> responseApiPeidos = new ResponseAPI<OrdersReaderDto>();


                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Orders/{id}");


                // var url = $"api/Orders/{DocEntry}";


                var response = await http.GetFromJsonAsync<ResponseAPI<OrdersReaderDto>>($"api/Orders/{DocEntry}");



                if (response.Valor != null)
                {
                    responseApiPeidos.Errores = response.Errores;
                    responseApiPeidos.EsCorrecto = response.EsCorrecto;
                    responseApiPeidos.Mensaje = response.Mensaje;
                    responseApiPeidos.CantRegistros = response.CantRegistros;
                    responseApiPeidos.Valor=response.Valor;

                    return responseApiPeidos;
                }
                else
                {
                    return new ResponseAPI<OrdersReaderDto> { EsCorrecto = false, Mensaje = "No se encontró el pedido." };
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<OrdersReaderDto> { EsCorrecto = false, Mensaje = ex.Message };
            }


        }

        public async Task<ResponseAPI<OrdersReaderDto>> UpdatePedidoAsync(PedidoUpdateDto pedidoUpdateDto)
        {
            try
            {
                ResponseAPI<OrdersReaderDto> responseApiPeidos = new ResponseAPI<OrdersReaderDto>();


                var response = await http.PatchAsJsonAsync($"api/Orders/{pedidoUpdateDto.DocEntry}", pedidoUpdateDto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<OrdersReaderDto>>();
                    return result ?? new ResponseAPI<OrdersReaderDto>();
                }
                else
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ResponseAPI<OrdersReaderDto>>();
                    return errorResult ?? new ResponseAPI<OrdersReaderDto>();
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<OrdersReaderDto> { EsCorrecto = false, Mensaje = ex.Message };
            }
        }
        public async Task<ResponseAPI<bool>>CancelPedidoAsync(int docEntry)
        {
            try
            {

                //var request = new HttpRequestMessage(HttpMethod.Patch, "https://localhost:8888/api/Orders/1");
                // Enviar un cuerpo vacío para indicar que no se están enviando datos adicionales
                var content = new StringContent("",Encoding.UTF8,"application/json");

                var response = await http.PatchAsync($"api/Orders/cancelar/{docEntry}",content);

                if (response.IsSuccessStatusCode)
                {
                    var result =await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                    return result ?? new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se recibió respuesta."
                    };
                }

                var errorResult =await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return errorResult ?? new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al cancelar pedido."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Valor = false
                };
            }
        }


    }

}
 