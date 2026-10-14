using Microsoft.AspNetCore.Mvc;
using Miluc.Client.Interfaces.Nomina;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.CentroDeCostos;

using Miluc.Shared.Models.Response;
using System.Text;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class CentroCostosController(ICentroCostosService _centroCostosService, ILogService _log) : Controller
    {


        [HttpGet("CentroCostos")]
        public async Task<ActionResult<ResponseAPI<List<CentroCostosReaderDto>>>> GetCentroCostosAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (centroCostos, totalRegistros) = await _centroCostosService.GetCentroCostosAsync(filtro, page, cantidad);

                // Si la lista es nula o vacía, devolvemos OK con lista vacía y 0 registros (no 404)
                if (centroCostos == null || centroCostos.Count == 0)
                {
                    return Ok(new ResponseAPI<List<CentroCostosReaderDto>>
                    {
                        EsCorrecto = true,
                        Valor = [],
                        Mensaje = "No se encontraron centros de costos.", // CORREGIDO
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<CentroCostosReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = centroCostos,
                    Mensaje = "Centros de costos obtenidos correctamente.", // CORREGIDO
                    CantRegistros = totalRegistros
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(GetCentroCostosAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "centrocostos");

                return StatusCode(500, new ResponseAPI<List<CentroCostosReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = [],
                    Mensaje = "Ocurrió un error al obtener los centros de costos.",
                    CantRegistros = 0
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> UpsertCentroCostosAsync(int id, [FromBody] UpsertCentroCostosDto upsertCentroCostos)
        {
            if (upsertCentroCostos == null)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Debe enviar la información de el centro de costos.",
                    CantRegistros = 0
                });
            }

            // Si viene ID en la ruta pero el DTO trae 0, se sincronizan; si ambos traen IDs distintos, se rechaza.
            if (upsertCentroCostos.CentroCostoId != 0 && id != upsertCentroCostos.CentroCostoId)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "El Id de la URL no coincide con el Id enviado.",
                    CantRegistros = 0
                });
            }

            if (upsertCentroCostos.CentroCostoId == 0 && id > 0)
            {
                upsertCentroCostos.CentroCostoId = id;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Datos inválidos.",
                    CantRegistros = 0
                });
            }

            try
            {
                bool esNueva = upsertCentroCostos.CentroCostoId == 0;
                var resultado = await _centroCostosService.UpsertCentoCostosAsync(upsertCentroCostos);

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = resultado,
                    Mensaje = esNueva
                        ? "Centro de costos creado correctamente."
                        : "Centro de costos actualizado correctamente.",
                    CantRegistros = 1
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = ex.Message,
                    CantRegistros = 0
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = ex.Message,
                    CantRegistros = 0
                });
            }

            catch (Exception ex)
            {
                var errorReal = ObtenerMensajeDetallado(ex);

                await _log.GuardarErrorAsync(
                    message: errorReal,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(UpsertCentroCostosAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(CentroCostosController)
                );

                // Seguridad: Se registra el error detallado internamente, pero se responde un mensaje controlado al cliente.
                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Ocurrió un error interno al procesar la solicitud.",
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<CentroCostosReaderDto>>> GetCentroCostosByIdAsync(int id)
        {
            try
            {
                var centroCostos = await _centroCostosService.GetCentroCostosByIdAsync(id);
                if (centroCostos == null)
                {
                    return NotFound(new ResponseAPI<CentroCostosReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"No se encontró el centro de costos con ID {id}.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<CentroCostosReaderDto>
                {
                    EsCorrecto = true,
                    Valor = centroCostos,
                    Mensaje = "Centro de costos obtenido correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(GetCentroCostosByIdAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(CentroCostosController)
                );
                return StatusCode(500, new ResponseAPI<CentroCostosReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener el centro de costos.",
                    CantRegistros = 0
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteCentroCostosAsync(int id)
        {
            try
            {
                var resultado = await _centroCostosService.DeleteCentroCostosAsync(id);
                if (!resultado)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = $"No se encontró el centro de costos con ID {id}.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Centro de costos eliminado correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(DeleteCentroCostosAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(CentroCostosController)
                );

                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = ex.Message,
                    CantRegistros = 0
                });
            }
        }


        private static string ObtenerMensajeDetallado(Exception ex)
        {
            var sb = new StringBuilder(ex.Message);
            var actual = ex.InnerException;

            while (actual != null)
            {
                sb.Append(" | InnerException: ").Append(actual.Message);
                actual = actual.InnerException;
            }

            return sb.ToString();
        }
    }
}