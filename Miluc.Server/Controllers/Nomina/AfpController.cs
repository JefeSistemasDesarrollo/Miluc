using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.Models.Response;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Miluc.Server.Controllers.Nomina
{
    [Route("api/[controller]")]
    [ApiController]
    public class AfpController(IAfpService afpService, ILogService _log) : Controller
    {
        [HttpGet("Afp")]
        public async Task<ActionResult<ResponseAPI<List<AfpReaderDto>>>> GetAfpAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (afp, totalRegistros) = await afpService.GetAfpAsync(filtro, page, cantidad);
                if (afp == null || !afp.Any())
                {
                    return NotFound(new ResponseAPI<List<AfpReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron AFP.",
                        Valor = null,
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<AfpReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "AFP obtenidas exitosamente.",
                    Valor = afp,
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
                    ruta: "/api/Afp",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "Afp Controller"
                );

                return StatusCode(500, new ResponseAPI<List<AfpReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener AFP.",
                    CantRegistros = 0
                });
            }
        }
        [HttpPost("CrearAfp")]
        public async Task<ActionResult<ResponseAPI<AfpReaderDto>>> CreateAfpAsync(AfpCreateDto afpCreateDto)
        {
            // 1. Validación de integridad: Verifica que el objeto y sus campos esenciales no estén vacíos.
            if (afpCreateDto == null || string.IsNullOrWhiteSpace(afpCreateDto.Nombre))
                return BadRequest(new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "El nombre de la AFP es obligatorio y no puede estar vacío.",
                });

            try
            {
                // Invocación del servicio: Delega la lógica de negocio y persistencia.
                var nuevaAfp = await afpService.CreateAfpAsync(afpCreateDto);

                // Respuesta exitosa: Retorna el DTO procesado con su nuevo ID.
                return Ok(new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = true,
                    Mensaje = "AFP creada exitosamente.",
                    Valor = nuevaAfp,
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                // Registro de auditoría: Almacena el error detallado para soporte técnico.
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/Afp/CrearAfp",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "Afp Controller"
                );

                // Control de excepciones: Retorna el mensaje de error específico al usuario.
                return StatusCode(500, new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Ocurrió un error al crear la AFP: {ex.Message}",
                    CantRegistros = 0
                });
            }
        }
    }
}