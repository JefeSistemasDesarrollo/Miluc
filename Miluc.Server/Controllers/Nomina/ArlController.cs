using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.ArlDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
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
            [HttpGet("{id}")]
            public async Task<ActionResult<ResponseAPI<ArlReaderDto>>> GetByArlAsync(int id)


            {
                try
                {
                    var arl = await arlService.GetByArlAsync(id);
                    if (arl == null)
                    {
                        return NotFound(new ResponseAPI<ArlReaderDto>
                        {
                            EsCorrecto = false,
                            Valor = null,
                            Mensaje = "eps no encontrado",
                            CantRegistros = 0,
                        });
                    }
                    return Ok(new ResponseAPI<ArlReaderDto>
                    {
                        EsCorrecto = true,
                        Valor = arl,
                        Mensaje = "arl obtenido correctamente",
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
                        ruta: $"/api/Arl/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: "ArlController");
                    return StatusCode(500, new ResponseAPI<ArlReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "Ocurrió un error al obtener el eps.",
                        CantRegistros = 0
                    });
                }
            }


            [HttpPut("{id}")]
            public async Task<ActionResult<ResponseAPI<ArlReaderDto>>> UpdateArlAsync(int id, [FromBody] ArlUpdate arlUpdate)
            {
                // 1. Validaciones previas en un solo bloque 
                if (arlUpdate == null)
                    return BadRequest(ErrorResponse("Se debe enviar la información de la ARL."));

                if (id != arlUpdate.ArlId)
                    return BadRequest(ErrorResponse("El ID enviado en la URL no coincide con la ARL."));

                if (!ModelState.IsValid)
                {
                    var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ErrorResponse("Datos inválidos.", errores));
                }

                try
                {
                    var arlActualizada = await arlService.UpdateArlAsync(arlUpdate);

                    return arlActualizada is null
                        ? NotFound(ErrorResponse("No se encontró la EPS para actualizar."))
                        : Ok(new ResponseAPI<ArlReaderDto>
                        {
                            EsCorrecto = true,
                            Valor = arlActualizada,
                            Mensaje = "EPS actualizada correctamente.",
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
                        ruta: $"/api/Arl/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(ArlController)
                    );

                    return BadRequest(ErrorResponse(ex.Message));
                }
            }



        private static ResponseAPI<ArlReaderDto> ErrorResponse(string mensaje, List<string>? errores = null) => new()
        {
            EsCorrecto = false,
            Valor = null,
            Mensaje = mensaje,
            Errores = errores,
            CantRegistros = 0
        };

    }
    }
