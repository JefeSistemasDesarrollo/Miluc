using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto;


using Miluc.Shared.Models.Response;
namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class EsquemaVacunacionController(IEsquemaVacunacionService _esquemaService, ILogService _log) : Controller
    {
        [HttpGet("Esquemas")]
        public async Task<ActionResult<ResponseAPI<List<EsquemaVacunacionReaderDto>>>> GetEsquemasVacunacion([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (esquemas, totalRegistros) = await _esquemaService.GetEsquemasVacunacionAsync(filtro, page, cantidad);

                if (esquemas == null || esquemas.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron esquemas de vacunación.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = esquemas,
                    Mensaje = "Esquemas de vacunación obtenidos correctamente.",
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
                    ruta: "/api/Vacunas/Esquemas",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "VacunasController");

                return StatusCode(500, new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        [HttpPost("CreateEsquemaVacunacion")]
        public async Task<ActionResult<ResponseAPI<bool>>> CreateEsquemaVacunacionAsync([FromBody] List<CreateEsquemaVacunacionDto> esquemaVacunacion)
        {
            
            if (esquemaVacunacion == null || !esquemaVacunacion.Any())
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "La lista de esquemas de vacunación enviada está vacía.",
                    CantRegistros = 0
                });
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Uno o más campos de la lista de esquemas de vacunación no cumplen con las validaciones requeridas.",
                    CantRegistros = 0
                });
            }

            try
            {

                var resultado = await _esquemaService.CreateEsquemaVacunacionAsync(esquemaVacunacion);

                if (!resultado)
                {
                    return BadRequest(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "No fue posible registrar la información de vacunación en el sistema.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Información de vacunación registrada correctamente.",
                    CantRegistros = esquemaVacunacion.Count // Muestra cuántos insertó en total
                });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/Vacunas/CreateEsquemaVacunacion",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(VacunasController));

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = $"Error interno al registrar la información de vacunación: {ex.Message}",
                        CantRegistros = 0
                    });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<List<EsquemaVacunacionReaderDto>>>> GetEsquemaVacunacionByIdAsync(int id)
        {

            try
            {
                var infoFamiliar = await _esquemaService.GetEsquemaVacunacionByIdAsync(id);
                if (infoFamiliar == null)
                {
                    return NotFound(new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron registros de esquemas de vacunación.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = infoFamiliar,
                    Mensaje = "Información de esquema de vacunación obtenida correctamente.",
                    CantRegistros = infoFamiliar.Count
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                     message: ex.Message,
                     StackTrace: ex.StackTrace,
                     usuario: User.Identity?.Name ?? "Sistema",
                     metodo: "HttpGet",
                     ruta: $"/api/EsquemasVacunacion",
                     ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                     origen: "EsquemaVacunacionController");
                return StatusCode(500, new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error al obtener la información de esquemas de vacunación: {ex.Message}",
                    CantRegistros = 0
                });


            }

        }

    }

}
