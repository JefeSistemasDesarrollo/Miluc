using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.Ocrd;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Sap.Cliente;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]

    public class OcrdController(ISapOcrdService _sapClienteService, ILogService _log) : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status500InternalServerError)]
       
        [PermissionAuthorize("Cliente.View")]
        public async Task<ActionResult<ResponseAPI<List<SapClienteReaderDto>>>> GetAllClientesAsync([FromQuery] string? buscar, [FromQuery] int? pagina = null, [FromQuery] int? cantidad = null,
            [FromQuery] int? codVendedorSAP = null)
        {
            try
            {
                var response = await _sapClienteService.GetallClienteAsync(buscar, pagina, cantidad, codVendedorSAP);

                return Ok(new ResponseAPI<List<SapClienteReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de clientes",
                    Valor = response.Data,
                    CantRegistros = response.TotalRegistros
                });
            }
            catch (Exception ex)
            {
            

                await _log.GuardarErrorAsync(message: ex.Message,
                              StackTrace: ex.StackTrace,
                              usuario: User.Identity?.Name ?? "Sistema",
                              metodo: nameof(GetAllClientesAsync),
                              ruta: HttpContext.Request.Path,
                              ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                              origen: nameof(OcrdController));

                return StatusCode(500, new ResponseAPI<List<SapClienteReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje ="Ha ocurrido un error interno alc onsultar el cliente",
                    Errores =[ex.Message]
                });
            }
        }


        [HttpGet("{cardcode}")]
        [PermissionAuthorize("Cliente.Detail")]
        public async Task<ActionResult<ResponseAPI<SapClienteReaderDto>>> GetByIdClienteAsync(string cardcode)
        {
            try
            {
                var cliente = await _sapClienteService.GetByIdClienteAsync(cardcode);
                if (cliente == null)
                {
                    return NotFound(new ResponseAPI<SapClienteReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Cliente no encontrado"
                    });
                }
                return Ok(new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = true,
                    Valor = cliente,
                    Mensaje = "Cliente obtenido correctamente"
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                          message: ex.Message,
                           StackTrace: ex.StackTrace,
                           usuario: User.Identity?.Name ?? "Sistema",
                           metodo: "HttpGet",
                           ruta: $"/api/Usuario​/{cardcode}",
                           ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                           origen: $"UsuarioController");
                return StatusCode(500, new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        [HttpPost]
        [PermissionAuthorize("Cliente.Create")]
        public async Task<ActionResult<ResponseAPI<SapClienteReaderDto>>> CreateClienteAsync([FromBody] SapClienteCreateEditDto sapClienteCreateDto)
        {
            try
            {
                var response = await _sapClienteService.CreateClienteAsync(sapClienteCreateDto);

                if (response.CardCode == null)
                {
                    return NotFound(new ResponseAPI<SapClienteReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"los datos son invalidos {response.ToString()} ",
                        Errores = new List<string> { response.ToString() }
                    });
                }
                return Ok(new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = true,
                    Valor = response,
                    Mensaje = "Cliente Creado Correctamente"
                });
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new ResponseAPI<SapClienteReaderDto>
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
                      metodo: nameof(CreateClienteAsync),
                      ruta: HttpContext.Request.Path,
                      ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                      origen: nameof(OcrdController));

                return StatusCode(500, new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = [ex.Message]
                });
            }
        }
        [HttpPut("{cardCode}")]
        [PermissionAuthorize("Cliente.Update")]
        public async Task<ActionResult<ResponseAPI<SapClienteReaderDto>>> ActualizarClienteAsync(string cardCode, [FromBody] SapClienteCreateEditDto sapClienteCreateDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cardCode))
                {
                    return BadRequest(new ResponseAPI<SapClienteReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "El código del cliente es obligatorio.",
                        Errores = new List<string> { "El código del cliente es obligatorio." }
                    });
                }

                // Garantizamos que el CardCode que se actualizará sea el de la URL
                sapClienteCreateDto.CardCode = cardCode;

                var response = await _sapClienteService.EditarClienteAsunc(sapClienteCreateDto);

                if (response == null || string.IsNullOrWhiteSpace(response.CardCode))
                {
                    return NotFound(new ResponseAPI<SapClienteReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No fue posible actualizar el cliente.",
                        Errores = new List<string> { "No fue posible actualizar el cliente." }
                    });
                }

                return Ok(new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = true,
                    Valor = response,
                    Mensaje = "Cliente actualizado correctamente."
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseAPI<SapClienteReaderDto>
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
                    metodo: nameof(ActualizarClienteAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(OcrdController));

                return StatusCode(500, new ResponseAPI<SapClienteReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        [HttpDelete("{cardCode}")]
        [PermissionAuthorize("Cliente.Delete")]
        public async Task<ActionResult<ResponseAPI<bool>>> EliminarClienteAsync(string cardCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cardCode))
                {
                    return BadRequest(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "El código del cliente es obligatorio",
                        Errores = new List<string> { "El código del cliente es obligatorio" }

                    });
                }

                var response = await _sapClienteService.EliminarClienteAsync(cardCode);

                if (!response)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "No fue posible eliminar el cliente.",
                        Errores = new List<string> { "No fue posible eliminar el cliente." }
                    });


                }
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Cliente eliminado correctamente."
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseAPI<bool>
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
                 metodo: nameof(EliminarClienteAsync),
                 ruta: HttpContext.Request.Path,
                 ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                 origen: nameof(OcrdController));

                string mensaje = ex.Message;
                if (mensaje.Contains("You cannot remove business partner", StringComparison.OrdinalIgnoreCase))
                {
                    mensaje = "No es posible eliminar el cliente porque tiene documentos asociados (Pedidos de Venta, Facturas, Entregas, Pagos u otros documentos en SAP).";
                }
                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = mensaje,
                    Errores = new List<string> { ex.Message }
                });


            }
        }
    }
}
