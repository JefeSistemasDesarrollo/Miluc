using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.Ordr;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Sap.Pedidos;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(ISapOrdrService _sapOrdrService, ILogService _log) : Controller
    {

        [HttpPost]
        [PermissionAuthorize("Orders.Create")]
        public async Task<ActionResult<ResponseAPI<OrdersReaderDto>>> CreatePedido([FromBody] PedidoCreateDto pedidoCreateDto)
        {
            try
            {
                var response = await _sapOrdrService.CreatePedidoAsyc(pedidoCreateDto);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(new ResponseAPI<OrdersReaderDto>
                {
                    EsCorrecto = true,
                    Mensaje = "Pedido creado correctamente.",
                    Valor = response
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseAPI<OrdersReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(CreatePedido),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(OrdersController));

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseAPI<OrdersReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Ocurrió un error interno.",
                        Errores = new List<string> { ex.Message }
                    });
            }
        }

        [HttpGet]
        [PermissionAuthorize("Orders.View")]
        public async Task<ActionResult<ResponseAPI<List<OrdersReaderDto>>>> ListarPedidosAsync([FromQuery] string? buscar = null, [FromQuery] int? pagina = null, [FromQuery] int? cantidad = null, [FromQuery] DateTime? fechaInicio = null, [FromQuery] DateTime? fechaFin = null)
        {
            try
            {
                var response = await _sapOrdrService.ListarPedidosAsync(buscar, pagina, cantidad,fechaInicio, fechaFin);


                if (response.data == null)
                {

                    return NotFound(new ResponseAPI<(List<OrdersReaderDto> data, int TotalRegistros)>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron pedidos.",
                        Errores = new List<string> { "No se encontraron pedidos." },
                        Valor = (new List<OrdersReaderDto>(), 0)
                    });

                }




                return Ok(new ResponseAPI<List<OrdersReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Pedidos encontrados.",
                    Errores = new List<string>(),
                    Valor = response.data,
                    CantRegistros = response.TotalRegistros



                });


            }

            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseAPI<OrdersReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }

            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                      message: ex.Message,
                       StackTrace: ex.StackTrace,
                       usuario: User.Identity?.Name ?? "Sistema",
                       metodo: "HttpGet",
                       ruta: $"/api/Orders​",
                       ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                       origen: $"OrdersController");


                return StatusCode(500, new ResponseAPI<(List<OrdersReaderDto> data, int TotalRegistros)>
                {
                    EsCorrecto = false,
                    Mensaje = "Ocurrió un error al listar los pedidos.",
                    Errores = new List<string> { ex.Message },
                    Valor = (new List<OrdersReaderDto>(), 0)
                });
            }
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("Orders.Detail")]
        public async Task<ActionResult<ResponseAPI<OrdersReaderDto>>> GetOrderByIdAsync(int id)
        {
            try
            {
                var response = await _sapOrdrService.GetOrderByIdAsync(id);

                if (response == null)
                {
                    return NotFound(new ResponseAPI<OrdersReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Pedido no encontrado.",
                        Errores = new List<string> { "Pedido no encontrado." },
                        Valor = null
                    });
                }
                return Ok(new ResponseAPI<OrdersReaderDto>
                {
                    EsCorrecto = true,
                    Mensaje = "Pedido encontrado.",
                    Errores = new List<string>(),
                    Valor = response
                });
            }
            catch (Exception ex)
            {


                await _log.GuardarErrorAsync(
              message: ex.Message,
              StackTrace: ex.StackTrace,
              usuario: User.Identity?.Name ?? "Sistema",
              metodo: nameof(GetOrderByIdAsync),
              ruta: HttpContext.Request.Path,
              ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
              origen: nameof(OrdersController));



                return StatusCode(500, new ResponseAPI<OrdersReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Ocurrió un error al obtener el pedido.",
                    Errores = new List<string> { ex.Message },
                    Valor = null
                });
            }
        }

        [HttpPatch("{docEntry:int}")]
        [PermissionAuthorize("Orders.Update")]
        public async Task<ActionResult<ResponseAPI<OrdersReaderDto>>> UpdateOrderAsync(int docEntry, [FromBody] PedidoUpdateDto pedidoUpdateDto)
        {
            try
            {
                // VALIDAR BODY

                if (pedidoUpdateDto == null)
                {
                    return BadRequest(
                        new ResponseAPI<OrdersReaderDto>
                        {
                            EsCorrecto = false,
                            Mensaje = "Datos inválidos.",
                            Errores = new List<string> { "El body es requerido" }
                        });
                }

                // VALIDAR CONSISTENCIA

                if (docEntry != pedidoUpdateDto.DocEntry)
                {
                    return BadRequest(
                        new ResponseAPI<OrdersReaderDto>
                        {
                            EsCorrecto = false,
                            Mensaje = "DocEntry inválido.",
                            Errores = new List<string> { "El DocEntry no coincide" }
                        });
                }

                // ACTUALIZAR

                var response = await _sapOrdrService.UpdatePedidoAsync(pedidoUpdateDto);

                return Ok(new ResponseAPI<OrdersReaderDto>
                {
                    EsCorrecto = true,
                    Mensaje = "Pedido actualizado correctamente.",
                    Errores = new List<string>(),
                    Valor = response
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(UpdateOrderAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection
                        .RemoteIpAddress?.ToString(),
                    origen: nameof(OrdersController));

                return StatusCode(500, new ResponseAPI<OrdersReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje =
                            "Ocurrió un error al actualizar el pedido.",
                    Errores = new List<string> { ex.Message },
                    Valor = null
                });
            }
        }

        [HttpPatch("cancelar/{docEntry:int}")]
        [PermissionAuthorize("Orders.Cancelar")]
        public async Task<ActionResult<ResponseAPI<bool>>> CancelPedidoAsync(int docEntry)
        {
            try
            {
                var response =await _sapOrdrService.CancelPedidoAsync(docEntry);

                return Ok(
                    new ResponseAPI<bool>
                    {
                        EsCorrecto = response,
                        Mensaje = response
                            ? "Pedido cancelado correctamente."
                            : "No se pudo cancelar el pedido.",
                        Valor = response
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = ex.Message,
                        Valor = false
                    });
            }

        }

    }
}
