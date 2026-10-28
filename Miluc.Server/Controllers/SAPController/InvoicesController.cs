using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.Oinv;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Sap.Cliente;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoicesController(ISapOinvServices _trazabilidadService, ILogService _log) : Controller
    {
        [HttpGet("pedido-desde-factura/{docEntry:int}")]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapClienteReaderDto>>), StatusCodes.Status500InternalServerError)]
        [PermissionAuthorize("TrazabilidadFactura.View")]
        public async Task<ActionResult<ResponseAPI<int>>> ObtenerPedidoDesdeFactura(int docEntry)
        {
            try
            {
                if (docEntry <= 0)
                    return BadRequest("El DocEntry de la factura no es válido.");

                int numeroPedido =
                    await _trazabilidadService.ObtenerNumeroPedidoDesdeFactura(docEntry);

                if (numeroPedido == null)
                {
                    return NotFound(new ResponseAPI<int>
                    {
                        CantRegistros = 0,
                        Mensaje = "No encontrado",
                        EsCorrecto = false,
                    });
                }

                return Ok(new ResponseAPI<int>
                {
                    EsCorrecto = true,
                    Mensaje = "Orden obtenida correctamente",
                    Valor = Convert.ToInt32(numeroPedido)
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                        message: ex.Message,
                         StackTrace: ex.StackTrace,
                         usuario: User.Identity?.Name ?? "Sistema",
                         metodo: "HttpGet",
                         ruta: $"/api/ObtenerPedidoDesdeFactura​/{docEntry}",
                         ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                         origen: $"ObtenerPedidoDesdeFactura");
                return StatusCode(500, new ResponseAPI<int>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
    }
}
