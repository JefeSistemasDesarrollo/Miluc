using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.AfpDto;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
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
                if (afp == null || afp.Count == 0)
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
                    Mensaje = ex.Message,
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<AfpReaderDto>>> GetByAfpAsync(int id)


        {
            try
            {
                var afp = await afpService.GetByAfpAsync(id);
                if (afp == null)
                {
                    return NotFound(new ResponseAPI<ArlReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "AFP no encontrado",
                        CantRegistros = 0,
                    });
                }
                return Ok(new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = true,
                    Valor = afp,
                    Mensaje = "AFP obtenido correctamente",
                    CantRegistros = 1,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR REAL EN GET]: {ex.Message} -> {ex.StackTrace}");
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: $"/api/Afp/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "AfpController");
                return StatusCode(500, new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener la Afp.",
                    CantRegistros = 0
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<AfpReaderDto>>> UpdateAfpAsync(int id, [FromBody] UpdateAfp updateAfp)
        {
            // 1. Validaciones previas en un solo bloque 
            if (updateAfp == null)
                return BadRequest(ErrorResponse("Se debe enviar la información de la AFP."));

            if (id != updateAfp.AfpId)
                return BadRequest(ErrorResponse("El ID enviado en la URL no coincide con la AFP."));

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ErrorResponse("Datos inválidos.", errores));
            }

            try
            {
                var afpActualizada = await afpService.UpdateAfpAsync(updateAfp);

                return afpActualizada is null
                    ? NotFound(ErrorResponse("No se encontró la AFp para actualizar."))
                    : Ok(new ResponseAPI<AfpReaderDto>
                    {
                        EsCorrecto = true,
                        Valor = afpActualizada,
                        Mensaje = "Afp actualizada correctamente.",
                        CantRegistros = 1
                    });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPut",
                    ruta: $"/api/Afp/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(AfpController)
                );

                return BadRequest(ErrorResponse(ex.Message));
            }
        }


        private static ResponseAPI<AfpReaderDto> ErrorResponse(string mensaje, List<string>? errores = null) => new()
        {
            EsCorrecto = false,
            Valor = null,
            Mensaje = mensaje,
            Errores = errores,
            CantRegistros = 0
        };
    
     [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteAfpAsync(int id)
        {
            try
            {
                var result = await afpService.DeleteAfpAsync(id);

                if (!result)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "No se encontró la Afp.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Afp eliminada correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.ToString(),
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "DeleteAfpAsync", // Es recomendable poner el nombre real del método
                    ruta: $"/api/Afp/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "AfpController"
                );

                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = ex.Message,
                    CantRegistros = 0
                });
            }
        }   
    }
}

    
