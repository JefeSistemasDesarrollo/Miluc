using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.ConexionSap;
using Miluc.Server.Interfaces.Sap.Oitm;
using Miluc.Server.Interfaces.Sap.Ordr;
using Miluc.Shared.DTOs.Sap.Articulos;
using Miluc.Shared.DTOs.Sap.Pedidos;
using Newtonsoft.Json;
using System.Text;

namespace Miluc.Server.Servicios.SapService
{
    public class SapOrderService(SapDbContex _sapDbContex, IConexionServiceLayer _serviceConexion, ISapOitmService _sapOitmService) : ISapOrdrService
    {
        public async Task<OrdersReaderDto> CreatePedidoAsyc(PedidoCreateDto pedidoCreateDto)
        {
            var credenciales = await _serviceConexion.ConexionSapService();
            try
            {
                //validaciones 
                if (credenciales.Valor == null)
                {
                    throw new ArgumentNullException("No se encontraron las credenciales de SAP en la base de datos");
                }

                if (credenciales.Valor == null)
                {
                    throw new ArgumentNullException("No se encontraron las credenciales de SAP en la base de datos");
                }
                if (string.IsNullOrEmpty(pedidoCreateDto.CardCode))
                {
                    throw new ArgumentNullException("El cliente es inválido");
                }
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using var httpClient = new HttpClient(clientHandler);

                if (credenciales.Valor == null)
                {
                    credenciales = await _serviceConexion.ConexionSapService();
                }

                httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={credenciales.Valor.B1SESSION}; RouteId={credenciales.Valor.ROUTEID}");

                string url = $"{credenciales.Valor.URLServiceLayer}/Orders";

                var json = JsonConvert.SerializeObject(pedidoCreateDto);

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, data);
                var contect = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<OrdersReaderDto>(contect);

                    return result;
                }
                else
                {
                    return new OrdersReaderDto
                    {
                        CardCode = "",
                        CardName = "",
                        DocNum = "",
                        DocEntry = 0,
                        DocRate = null,
                        DocTotal = null,
                        Address = "",
                        NumAtCard = "",
                        VatSum = null,
                        PaidToDate = null,
                        Comments = $"Error al crear el pedido: {contect}",
                        U_Picking = null,
                        U_PLACAS = "",
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el pedido: {ex.Message}", ex);
            }
            finally
            {
                await _serviceConexion.LogoutAsync(credenciales.Valor.URLServiceLayer, credenciales.Valor.B1SESSION, credenciales.Valor.ROUTEID);
            }
        }

        public async Task<OrdersReaderDto> UpdatePedidoAsync(PedidoUpdateDto pedidoUpdateDto)
        {
            var credenciales = await _serviceConexion.ConexionSapService();

            try
            {
                // VALIDACIONES

                if (string.IsNullOrEmpty(pedidoUpdateDto.CardCode))
                {
                    throw new Exception("El cliente es inválido");
                }


                if (credenciales?.Valor == null)
                {
                    throw new Exception(
                        "No se encontraron credenciales SAP");
                }

                if (pedidoUpdateDto == null)
                {
                    throw new Exception("El pedido es inválido");
                }

                if (pedidoUpdateDto.DocEntry <= 0)
                {
                    throw new Exception("El DocEntry es inválido");
                }

                if (string.IsNullOrEmpty(pedidoUpdateDto.CardCode))
                {
                    throw new Exception("El cliente es inválido");
                }

                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using var httpClient = new HttpClient(clientHandler);

                // COOKIE SAP

                httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={credenciales.Valor.B1SESSION}; RouteId={credenciales.Valor.ROUTEID}");

                // IMPORTANTE PARA PATCH DE LINEAS

                httpClient.DefaultRequestHeaders.Add("B1S-ReplaceCollectionsOnPatch", "true");

                // URL PATCH

                string url = $"{credenciales.Valor.URLServiceLayer}/Orders({pedidoUpdateDto.DocEntry})";

                var json = JsonConvert.SerializeObject(pedidoUpdateDto);

                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // PATCH

                var response = await httpClient.PatchAsync(url, data);

                var content = await response.Content.ReadAsStringAsync();

                // ERROR SAP

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = $"Error SAP: {content}";
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        { }
                        await _serviceConexion.LogoutAsync(credenciales.Valor.URLServiceLayer, credenciales.Valor.B1SESSION, credenciales.Valor.ROUTEID);

                        // throw new Exception(errorMessage);
                        throw new HttpRequestException($"SAP Service Layer rechazó la actualización de direcciones del cliente. Detalles: {errorMessage}");
                    }
                }

                // SAP DEVUELVE 204

                // CONSULTAR NUEVAMENTE EL PEDIDO

                return await GetOrderByIdAsync(pedidoUpdateDto.DocEntry);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al actualizar pedido: {ex.Message}",
                    ex
                );
            }
            finally
            {
                if (credenciales?.Valor != null)
                {
                    await _serviceConexion.LogoutAsync(
                        credenciales.Valor.URLServiceLayer,
                        credenciales.Valor.B1SESSION,
                        credenciales.Valor.ROUTEID
                    );
                }
            }
        }

        public async Task<OrdersReaderDto> GetOrderByIdAsync(int id)
        {
            try
            {
                var pedido = await _sapDbContex.ORDR.AsNoTracking()
                    .Include(x => x.OcrdClienteSap)
                    .Include(x => x.OSLP)
                    .Include(x => x.piking)
                    .Include(x => x.RDR1)
                    .Select(x => new OrdersReaderDto
                    {
                        CardCode = x.CardCode,
                        CardName = x.CardName,
                        ListName = x.OcrdClienteSap.OPLN.ListName,
                        CardFName = x.OcrdClienteSap.CardFName,
                        Domicilio = x.OcrdClienteSap.OCRG.GroupName,
                        NitCliente = x.OcrdClienteSap.LicTradNum,
                        DocNum = x.DocNum.ToString(),
                        DocEntry = x.DocEntry,
                        DocDate = x.DocDate,
                        DocDueDate = x.DocDueDate,
                        DocRate = x.DocRate,
                        DocTotal = x.DocTotal,
                        Address = x.Address,
                        NumAtCard = x.NumAtCard,
                        VatSum = x.VatSum,
                        PaidToDate = x.PaidToDate,
                        Comments = x.Comments ?? "",
                        U_Picking = x.U_Picking ?? 0,
                        U_PLACAS = x.piking != null && x.piking.Name != null ? x.piking.Name : "Sin placa",
                        CANCELED = x.CANCELED,
                        // SlpCode = x.SlpCode,
                        SlpCode = x.OSLP.SlpCode,
                        SlpName = x.OSLP.SlpName,
                        U_OrigenPedido = x.U_OrigenPedido,
                        detalle = x.RDR1.Select(d => new ArticuloPorListaDePreciosDto
                        {
                            ItemCode = d.ItemCode,
                            ItemName = d.Dscription,
                            Quantity = d.Quantity,
                            SWeight1 = d.OITM.SWeight1,
                            BuyUnitMsr = d.UnitMsr,
                            Rate = Convert.ToDecimal(d.VatPrcnt),
                            TaxCode = d.TaxCode,
                            // VatPrcnt = d.VatPrcnt,
                            //U_EquivalentedKg = d.U_EquivalentedKg,
                            //U_EquivalenteUni = d.U_EquivalenteUni,
                            // BaseSum = d.BaseSum
                            PriceAcobrar = d.Price,
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.DocEntry == id);
                //aca vamos hacer el detalle de la orden de venta
                if (pedido == null)
                {
                    throw new Exception($"No se encontró el pedido con el ID {id}");
                }
                if (pedido.detalle != null && pedido.detalle.Any())
                {
                    foreach (var linea in pedido.detalle)
                    {
                        var preciosActuales = await _sapOitmService.GetListOitmCarcodeItemcodeAsync(pedido.CardCode, linea.ItemCode, Convert.ToInt32(linea.Quantity));

                        if (preciosActuales != null)
                        {
                            // PRECIO ACTUAL DE LISTA
                            linea.PriceAsignado = preciosActuales.PriceAsignado;
                            // PRECIO ESPECIAL ACTUAL
                            linea.PriceEspecial = preciosActuales.PriceEspecial;
                            //// BANDERA UX
                            //linea.TienePrecioEspecial =
                            //    preciosActuales.PriceEspecial > 0;

                            //// OPCIONAL:
                            //// comparar si cambió el precio
                            //linea.PrecioCambio =
                            //    linea.PriceAcobrar !=
                            //    preciosActuales.PriceAcobrar;
                        }
                    }
                }
                return pedido;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el pedido: {ex.Message}", ex);
            }
        }
        public async Task<(List<OrdersReaderDto> data, int TotalRegistros)> ListarPedidosAsync(string? buscar = null, int? pagina = null,
        int? cantidad = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 12;
                var query = _sapDbContex.ORDR.AsNoTracking().AsQueryable();
                // 1. Filtro por texto (Buscador)
                if (!string.IsNullOrEmpty(buscar))
                {
                    query = query.Where(x => x.CardCode.Contains(buscar) ||
                                             x.CardName.Contains(buscar) ||
                                             x.DocNum.ToString().Contains(buscar) ||
                                             x.U_PLACAS.ToString().Contains(buscar));
                }
                // 2. Filtro por Rango de Fechas (Garantizando horas extremas)
                if (fechaInicio.HasValue)
                {
                    DateTime inicio = fechaInicio.Value.Date; // 
                    query = query.Where(x => x.DocDate >= inicio);
                }
                if (fechaFin.HasValue)
                {
                    DateTime fin = fechaFin.Value.Date.AddDays(1).AddTicks(-1); // 23:59:59.999
                    query = query.Where(x => x.DocDate <= fin);
                }
                // 3. Contar registros totales aplicando todos los filtros previos
                int totalRegistros = await query.CountAsync();
                // 4. Paginación, Proyección y Ejecución
                var lista = await query.OrderByDescending(x => x.DocNum)
                    .Include(x => x.piking)
                    .Skip(((pagina ?? 1) - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .Select(x => new OrdersReaderDto
                    {
                        CardCode = x.CardCode,
                        CardName = x.CardName,
                        DocNum = x.DocNum.ToString(),
                        DocEntry = x.DocEntry,
                        DocDate = x.DocDate,
                        DocStatus = x.DocStatus,
                        CANCELED = x.CANCELED,
                        DocDueDate = x.DocDueDate,
                        DocRate = x.DocRate,
                        DocTotal = x.DocTotal,
                        Address = x.Address,
                        NumAtCard = x.NumAtCard,
                        VatSum = x.VatSum,
                        PaidToDate = x.PaidToDate,
                        Comments = x.Comments ?? "",
                        U_Picking = x.U_Picking ?? 0,
                        U_PLACAS = x.piking != null && x.piking.Name != null ? x.piking.Name : "Sin placa",
                    }).ToListAsync();

                return (lista, totalRegistros);
            }
            catch (Exception ex)
            {
                // Es mejor envolver la excepción original para no perder el StackTrace
                throw new Exception($"Error al obtener los pedidos: {ex.Message}", ex);
            }
        }
        public async Task<bool> CancelPedidoAsync(int docEntry)
        {
            var credenciales = await _serviceConexion.ConexionSapService();

            try
            {
                // VALIDACIONES
                if (credenciales?.Valor == null)
                {
                    throw new Exception("No se encontraron credenciales SAP");
                }
                if (docEntry <= 0)
                {
                    throw new Exception("El id del pedido es inválido");
                }
                // HANDLER HTTPS
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => true
                };
                using var httpClient = new HttpClient(clientHandler);
                httpClient.DefaultRequestHeaders.Add(
                    "Cookie",
                    $"B1SESSION={credenciales.Valor.B1SESSION}; RouteId={credenciales.Valor.ROUTEID}"
                );
                // URL
                string url = $"{credenciales.Valor.URLServiceLayer}/Orders({docEntry})/Cancel";
                // REQUEST PATCH
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = new StringContent("{}", Encoding.UTF8, "application/json")
                };
                // EJECUTAR REQUEST
                var response = await httpClient.SendAsync(request);
                // RESPUESTA SAP
                var content = await response.Content.ReadAsStringAsync();
                // VALIDAR ERROR
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error SAP: {content}");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al cancelar pedido: {ex.Message}",
                    ex
                );
            }
            finally
            {
                // LOGOUT SAP
                if (credenciales.Valor != null)
                {
                    await _serviceConexion.LogoutAsync(
                        credenciales.Valor.URLServiceLayer,
                        credenciales.Valor.B1SESSION,
                        credenciales.Valor.ROUTEID
                    );
                }
            }


        }
    }
}


