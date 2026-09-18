using Miluc.Client.Interfaces.SapInterfaces.TrazabilidadPedidos;
using Miluc.Client.Pages.PageSapOne.Comercial.Trazabilidad;
using Miluc.Shared.DTOs.Sap.Devolucion;
using Miluc.Shared.DTOs.Sap.Factura;
using Miluc.Shared.DTOs.Sap.NotaCredito;
using Miluc.Shared.DTOs.Sap.NotaDeEntrega;
using Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.SapService
{
    public class SapTrazabilidadClientServices(HttpClient _http) : ISapTrazabilidadPedidos
    {
        public async Task<ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>>> ObtenerDetalleDevolucion(int DocEntry)
        {
            try
            {
                ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>> response = new ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>>();
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Trazabilidad/nota-detalle/1");
                var result = await _http.GetFromJsonAsync<ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>>>($"api/Trazabilidad/devolucion-detalle/{DocEntry}");
                if (result != null)
                {
                    response.EsCorrecto = true;
                    response.Valor = result.Valor;
                    response.Mensaje = result.Mensaje;
                    response.CantRegistros = result.Valor.Count;
                }
                else
                {
                    response.EsCorrecto = false;
                    response.Valor = null;
                    response.Mensaje = "No se encontró detalle de la devolución ";

                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el detalle de la Devolución{ex.Message}");
            }
        }

        public async Task<ResponseAPI<List<Inv1DetalleReaderDto>>> ObtenerDetalleFactura(int DocEntry)
        {
            try
            {
                ResponseAPI<List<Inv1DetalleReaderDto>> response = new ResponseAPI<List<Inv1DetalleReaderDto>>();
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Trazabilidad/nota-detalle/1");
                var result = await _http.GetFromJsonAsync<ResponseAPI<List<Inv1DetalleReaderDto>>>($"api/Trazabilidad/factura-detalle/{DocEntry}");
                if (result != null)
                {
                    response.EsCorrecto = true;
                    response.Valor = result.Valor;
                    response.Mensaje = result.Mensaje;
                    response.CantRegistros = result.Valor.Count;
                }
                else
                {
                    response.EsCorrecto = false;
                    response.Valor = null;
                    response.Mensaje = "No se encontro detalle la factura";

                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el detalle de la Factura{ex.Message}");
            }
        }
        public async Task<ResponseAPI<List<Rin1DetalleNotaCredito>>> ObtenerDetalleNotaCredito(int DocEntry)
        {
            try
            {
                ResponseAPI<List<Rin1DetalleNotaCredito>> response = new ResponseAPI<List<Rin1DetalleNotaCredito>>();
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Trazabilidad/nota-detalle/1");
                var result = await _http.GetFromJsonAsync<ResponseAPI<List<Rin1DetalleNotaCredito>>>($"api/Trazabilidad/notacredito-detalle/{DocEntry}");
                if (result != null)
                {
                    response.EsCorrecto = true;
                    response.Valor = result.Valor;
                    response.Mensaje = result.Mensaje;
                    response.CantRegistros = result.Valor.Count;
                }
                else
                {
                    response.EsCorrecto = false;
                    response.Valor = null;
                    response.Mensaje = "No se encontro detalle";

                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el detalle de la nota de entrega{ex.Message}");
            }
        }
        public async Task<ResponseAPI<List<Dln1DetalleReaderDto>>> ObtenerDetalleNota(int DocEntry)
        {
            try
            {
                ResponseAPI<List<Dln1DetalleReaderDto>> response=new ResponseAPI<List<Dln1DetalleReaderDto>>();   
                 //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Trazabilidad/nota-detalle/1");
                 var result = await _http.GetFromJsonAsync<ResponseAPI<List<Dln1DetalleReaderDto>>>($"api/Trazabilidad/nota-detalle/{DocEntry}");
                if (result != null)
                {
                    response.EsCorrecto = true;
                    response.Valor = result.Valor;
                    response.Mensaje= result.Mensaje;
                    response.CantRegistros = result.Valor.Count;
                }
                else
                {
                    response.EsCorrecto = false;
                    response.Valor = null;
                    response.Mensaje = "No se encontro detalle";
                
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el detalle de la nota de entrega{ex.Message}");
            }
        }

      
        public async Task<ResponseAPI<TrazabilidadOrdenReaderDto>> ObtenerTrazabilidadOrden(int DocNum)
        {
            try
            {
                ResponseAPI<TrazabilidadOrdenReaderDto> response = new ResponseAPI<TrazabilidadOrdenReaderDto>();
                var result = await _http.GetFromJsonAsync<ResponseAPI<TrazabilidadOrdenReaderDto>>($"api/Trazabilidad/orden-venta/{DocNum}");
               // var result = await _http.GetFromJsonAsync<ResponseAPI<TrazabilidadOrdenReaderDto>>($"api/Trazabilidad/{DocNum}");
              
                if (result.Valor != null)
                {
                    response.EsCorrecto = true;
                    response.Valor = result.Valor;
                    response.Mensaje = "Trazabilidad del pedido obtenida correctamente.";
                    response.CantRegistros = 1;
                }
                else
                {
                    response.EsCorrecto = false;
                    response.Mensaje = "No se encontró trazabilidad para el pedido especificado.";
                    response.CantRegistros = 0; 
                }

                return response;


            }
            catch (Exception ex) 
            {
                throw new Exception("Error al obtener la trazabilidad del pedido.", ex);
            }
        }
    }
}
