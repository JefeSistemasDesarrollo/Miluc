using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController] 
    [Route("api/[controller]")] 
    public class ArlController(IArlService arlService, ILogService _log) : Controller
    {
        [HttpGet("Arl")]
        public async Task<ActionResult<ResponseAPI<List<ArlReaderDto>>>> GetArlAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (arl, totalRegistros) = await arlService.GetArlAsync(filtro, page, cantidad);

                if (arl == null || !arl.Any())
                {
                    return NotFound(new ResponseAPI<List<ArlReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron ARL.",
                        Valor = null,
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<ArlReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "ARL obtenidas exitosamente.",
                    Valor = arl ,
                    CantRegistros = totalRegistros
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: "/api/Arl",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "Arl Controller"
                );

                return StatusCode(500, new ResponseAPI<List<ArlReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener las ARL.",
                    CantRegistros = 0
                });
            }
        }

        [HttpPost("CrearArl")] // Responde a: POST api/Arl/CrearArl
        public async Task<ActionResult<ResponseAPI<ArlReaderDto>>> CreateArlAsync(ArlCreateDto arlCreateDto)
        {
            if (arlCreateDto == null || string.IsNullOrWhiteSpace(arlCreateDto.Nombre) || string.IsNullOrWhiteSpace(arlCreateDto.Codigo))
            {
                return BadRequest(new ResponseAPI<ArlReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Los campos Nombre y Código son obligatorios.",
                    Valor = null,
                    CantRegistros = 0
                });
            }

            try
            {
                var arl = await arlService.CreateArlAsync(arlCreateDto);
                return Ok(new ResponseAPI<ArlReaderDto>
                {
                    EsCorrecto = true,
                    Mensaje = "ARL creada exitosamente.",
                    Valor = arl,
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/Arl/CrearArl",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "Arl Controller"
                );

                return StatusCode(500, new ResponseAPI<ArlReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Ocurrió un error al crear la ARL: {ex.Message}",
                    CantRegistros = 0
                });
            }
        }
    }
}