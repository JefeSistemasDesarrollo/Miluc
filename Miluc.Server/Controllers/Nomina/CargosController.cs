using Microsoft.AspNetCore.Mvc;
using Miluc.Client.Interfaces.Nomina;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.CargosDto;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.Models.Response;
using System.Text;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargosController (ICargoService _cargoService, ILogService _log) : Controller
    {
        

        [HttpGet("Cargos")]
        public async Task<ActionResult<ResponseAPI<List<CargosReaderDto>>>> GetCargosAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (cargo, totalRegistros) = await _cargoService.GetCargosAsync(filtro, page, cantidad);

                // Si la lista es nula o vacía, devolvemos OK con lista vacía y 0 registros (no 404)
                if (cargo == null || cargo.Count == 0)
                {
                    return Ok(new ResponseAPI<List<CargosReaderDto>> // CORREGIDO: List<CargosDto>
                    {
                        EsCorrecto = true,
                        Valor = [],
                        Mensaje = "No se encontraron cargos.", // CORREGIDO
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<CargosReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = cargo,
                    Mensaje = "Cargos obtenidos correctamente.", // CORREGIDO
                    CantRegistros = totalRegistros
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(GetCargosAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "cargos");

                return StatusCode(500, new ResponseAPI<List<CargosReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = [],
                    Mensaje = "Ocurrió un error al obtener los cargos.",
                    CantRegistros = 0
                });
            }
        }

     [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> UpsertCargoAsync(int id, [FromBody] UpsertCargos upsertCargos)
        {
            if (upsertCargos == null)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Debe enviar la información de el cargo.",
                    CantRegistros = 0
                });
            }

            // Si viene ID en la ruta pero el DTO trae 0, se sincronizan; si ambos traen IDs distintos, se rechaza.
            if (upsertCargos.CargoId != 0 && id != upsertCargos.CargoId)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "El Id de la URL no coincide con el Id enviado.",
                    CantRegistros = 0
                });
            }

            if (upsertCargos.CargoId == 0 && id > 0)
            {
                upsertCargos.CargoId = id;
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
                bool esNueva = upsertCargos.CargoId == 0;
                var resultado = await _cargoService.UpsertCargoAsync(upsertCargos);

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = resultado,
                    Mensaje = esNueva
                        ? "Cargo creado correctamente."
                        : "Cargo actualizado correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                var errorReal = ObtenerMensajeDetallado(ex);

                await _log.GuardarErrorAsync(
                    message: errorReal,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(UpsertCargoAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(CargosController)
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
        public async Task<ActionResult<ResponseAPI<CargosReaderDto>>> GetCargoByIdAsync(int id)
        {
            try
            {
                var cargo = await _cargoService.GetCargoByIdAsync(id);
                if (cargo == null)
                {
                    return NotFound(new ResponseAPI<CargosReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"No se encontró el cargo con ID {id}.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<CargosReaderDto>
                {
                    EsCorrecto = true,
                    Valor = cargo,
                    Mensaje = "Cargo obtenido correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(GetCargoByIdAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(CargosController)
                );
                return StatusCode(500, new ResponseAPI<CargosReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener el cargo.",
                    CantRegistros = 0
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteCargoAsync(int id)
        {
            try
            {
                var resultado = await _cargoService.DeleteCargoAsync(id);
                if (!resultado)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = $"No se encontró el cargo con ID {id}.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Cargo eliminado correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(DeleteCargoAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(EmpresaController)
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