using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.TrasabilidadOrdendeVenta;
using Miluc.Shared.DTOs.Sap.Cliente;
using Miluc.Shared.DTOs.Sap.Devolucion;
using Miluc.Shared.DTOs.Sap.Factura;
using Miluc.Shared.DTOs.Sap.NotaCredito;
using Miluc.Shared.DTOs.Sap.NotaDeEntrega;
using Miluc.Shared.DTOs.Sap.TrazabilidadOrdenesVenta;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
   // [Authorize]
    public class TrazabilidadController(ISapTrazabilidadPedido _sapTrazabilidadService, ILogService _log) : Controller
    {

 
[HttpGet("lista-trazabilidad")]
public async Task<ActionResult<ResponseAPI<List<TrazabilidadOrdenReaderDto>>>>
    ListaTrazabilidadVentas(string fechaInicial, string fechaFinal)
        {
            try
            {
                var listaTrazabilidad = await _sapTrazabilidadService
                    .ListaTrazabilidad(fechaInicial, fechaFinal);

                return Ok(listaTrazabilidad);
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(ListaTrazabilidadVentas),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(TrazabilidadController)
                );

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseAPI<List<TrazabilidadOrdenReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Ha ocurrido un error interno al consultar la trazabilidad de las ventas.",
                        Errores = [ex.Message]
                    }
                );
            }
        }



        [HttpGet("orden-venta/{DocNum:int}")]
        public async Task<ActionResult<ResponseAPI<TrazabilidadOrdenReaderDto>>> GetTrazabilidadOrdenVenta(int DocNum)
        {
            try
            {
                var Trasabilidad = await _sapTrazabilidadService.ObtenerTrazabilidadOrden(DocNum);

                if (Trasabilidad == null)
                {
                    return NotFound(new ResponseAPI<TrazabilidadOrdenReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"No se encontró la orden de venta con DocEntry: {DocNum}",
                        Valor = null
                    });
                }
                return Ok(new ResponseAPI<TrazabilidadOrdenReaderDto>
                {
                    EsCorrecto = true,
                    Mensaje = "Orden de venta encontrada exitosamente.",
                    Valor = Trasabilidad
                });
            }
            catch (Exception ex) 
            {
                await _log.GuardarErrorAsync(message: ex.Message,
                  StackTrace: ex.StackTrace,
                  usuario: User.Identity?.Name ?? "Sistema",
                  metodo: nameof(ObtenerDetalleNota),
                  ruta: HttpContext.Request.Path,
                  ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                  origen: nameof(TrazabilidadController));

                return StatusCode(500, new ResponseAPI<List<TrazabilidadOrdenReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Ha ocurrido un error interno al consultar la trazabilidad de la orden de venta ",
                    Errores = [ex.Message]
                });

            }

        }

       


        [HttpGet("nota-detalle/{DocEntry:int}")]
        public async Task<ActionResult<ResponseAPI<List<Dln1DetalleReaderDto>>>> ObtenerDetalleNota(int DocEntry)
        {
            try
            {
                var detalleNota = await _sapTrazabilidadService.ObtenerDetalleNotaDeEntrega(DocEntry);
                if (detalleNota == null)
                {
                    return NotFound(new ResponseAPI<List<Dln1DetalleReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se pudo encontrar el detalle la nota de entrega",
                    });
                }
                return Ok(new ResponseAPI<List<Dln1DetalleReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "",
                    Valor = detalleNota
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(message: ex.Message,
                              StackTrace: ex.StackTrace,
                              usuario: User.Identity?.Name ?? "Sistema",
                              metodo: nameof(ObtenerDetalleNota),
                              ruta: HttpContext.Request.Path,
                              ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                              origen: nameof(TrazabilidadController));

                return StatusCode(500, new ResponseAPI<List<SapClienteReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Ha ocurrido un error interno al consultar el detalle de la nota de entrega",
                    Errores = [ex.Message]
                });
            }

        }

        [HttpGet("devolucion-detalle/{DocEntry:int}")]
        public async Task<ActionResult<ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>>>> ObtenerDetalleDevolucion(int DocEntry)
        {
            try
            {
                var detalleDevolucion = await _sapTrazabilidadService.ObtenerDetalleDevolucion(DocEntry);
                if (detalleDevolucion == null)
                {
                    return NotFound(new ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se pudo encontrar el detalle la devolucion",
                    });
                }
                return Ok(new ResponseAPI<List<Rdn1DetalleDevolucionReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "",
                    Valor = detalleDevolucion
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(message: ex.Message,
                            StackTrace: ex.StackTrace,
                            usuario: User.Identity?.Name ?? "Sistema",
                            metodo: nameof(ObtenerDetalleNota),
                            ruta: HttpContext.Request.Path,
                            ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                            origen: nameof(TrazabilidadController));

                return StatusCode(500, new ResponseAPI<List<TrazabilidadController>>
                {
                    EsCorrecto = false,
                    Mensaje = "Ha ocurrido un error interno al consultar devolucion",
                    Errores = [ex.Message]
                });
            }
        }
        [HttpGet("factura-detalle/{DocEntry:int}")]
        public async Task<ActionResult<ResponseAPI<List<Inv1DetalleReaderDto>>>> ObtenerDetalleFactura(int DocEntry)
        {
            try
            {
                var detalleFactura = await _sapTrazabilidadService.ObtenerDetalleFactura(DocEntry);
                if (detalleFactura == null)
                {
                    return NotFound(new ResponseAPI<List<Inv1DetalleReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se pudo encontrar el detalle la fcatura",
                    });
                }
                return Ok(new ResponseAPI<List<Inv1DetalleReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "",
                    Valor = detalleFactura
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(message: ex.Message,
                              StackTrace: ex.StackTrace,
                              usuario: User.Identity?.Name ?? "Sistema",
                              metodo: nameof(ObtenerDetalleNota),
                              ruta: HttpContext.Request.Path,
                              ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                              origen: nameof(TrazabilidadController));

                return StatusCode(500, new ResponseAPI<List<Inv1DetalleReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Ha ocurrido un error interno al consultar el detalle de la fcatura",
                    Errores = [ex.Message]
                });
            }
        }
        [HttpGet("notacredito-detalle/{DocEntry:int}")]
        public async Task<ActionResult<ResponseAPI<List<Rin1DetalleNotaCredito>>>> ObtenerDetalleNotaCredito(int DocEntry)
        {
            try
            {
                var detalleNotaCredito= await _sapTrazabilidadService.ObtenerDetalleNotaCredito(DocEntry);
                if (detalleNotaCredito == null)
                {
                    return NotFound(new ResponseAPI<List<Rin1DetalleNotaCredito>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se pudo encontrar el detalle la fcatura",
                    });
                }
                return Ok(new ResponseAPI<List<Rin1DetalleNotaCredito>>
                {
                    EsCorrecto = true,
                    Mensaje = "",
                    Valor = detalleNotaCredito
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(message: ex.Message,
                              StackTrace: ex.StackTrace,
                              usuario: User.Identity?.Name ?? "Sistema",
                              metodo: nameof(ObtenerDetalleNota),
                              ruta: HttpContext.Request.Path,
                              ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                              origen: nameof(TrazabilidadController));

                return StatusCode(500, new ResponseAPI<List<Rin1DetalleNotaCredito>>
                {
                    EsCorrecto = false,
                    Mensaje = "Ha ocurrido un error interno al consultar el detalle de la fcatura",
                    Errores = [ex.Message]
                });
            }
        }
    }
}
