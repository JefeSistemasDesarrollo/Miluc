using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]

    public class AfiliacionSeguridadSocialController( IArlService arlService, ICajaCompensacionService cajaCompensacionService,
        IAfiliacionSeguridadSocialService afiliacionSeguridadSocialService
        , ILogService _log) : Controller
    {
        [HttpGet("AfiliacionSeguridadSocial")]
        public async Task<ActionResult<ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>>> GetAfiliacionSeguridadSocialAsync([FromQuery] string? filtro = null,[FromQuery] int page = 1, [FromQuery] int cantidad = 10) 
        {
            try
            {
                (List<AfiliacionSeguridadSocialreaderDto> afiliaciones, int totalRegistros) = await afiliacionSeguridadSocialService.GetAfiliacionSeguridadSocialAsync(filtro, page, cantidad);
                
               // (List<AfiliacionSeguridadSocialreaderDto> afiliaciones, int totalRegistros) =
                    await afiliacionSeguridadSocialService.GetAfiliacionSeguridadSocialAsync(filtro, page, cantidad);

                if (afiliaciones == null || afiliaciones.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron Afiliaciones.",
                        Valor = null,
                        CantRegistros = 0,
                    });
                }

                // 2. Retornamos la respuesta con los tipos y conteos reales corregidos
                return Ok(new ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Afiliaciones obtenidas correctamente.",
                    Valor = afiliaciones,
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
                    ruta: "/api/AfiliacionSeguridadSocial",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "AfiliacionSeguridadSocial Controller"
                );

                return StatusCode(500, new ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener las afiliaciones a seguridad social.",
                    CantRegistros = 0
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<AfiliacionSeguridadSocialreaderDto>>> UpdateAfiliacionAsync(int id, [FromBody] UpdateAFiliacionDto afiliacionUpdateDto)
        {
            // Validar null
            if (afiliacionUpdateDto == null)
            {
                return BadRequest(new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Debe enviar la información de la afiliación.",
                    CantRegistros = 0
                });
            }

            // Validar que el id de la URL coincida con el DTO
            if (id != afiliacionUpdateDto.AfiliacionId)
            {
                return BadRequest(new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "El id enviado no coincide.",
                    CantRegistros = 0
                });
            }

            // Validar modelo
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Campo = x.Key,
                        Errores = x.Value.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                return BadRequest(new ResponseAPI<object>
                {
                    EsCorrecto = false,
                    Valor = errores,
                    Mensaje = "Datos inválidos.",
                    CantRegistros = 0
                });
            }

            try
            {
                // Upsert devuelve bool -> no es un DTO
                var resultado = await afiliacionSeguridadSocialService.UpsertAfiliacionAsync(afiliacionUpdateDto);

                if (!resultado)
                {
                    return NotFound(new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontró o no se pudo actualizar la afiliación.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                {
                    EsCorrecto = true,
                    Valor = null, // El servicio solo retorna boolean, para devolver DTO se necesita consultar de nuevo
                    Mensaje = "Afiliación actualizada correctamente.",
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
                    ruta: $"/api/AfiliacionSeguridadSocial/UpdateAfiliacion/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "AfiliacionSeguridadSocialController"
                );

                return StatusCode(500, new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al actualizar la afiliación.",
                    CantRegistros = 0
                });
            }
        }
            [HttpGet("{id}")]
         
        public async Task<ActionResult<ResponseAPI<AfiliacionSeguridadSocialreaderDto>>> GetAfiliacionSeguridadSocialByIdAsync(int id)
            {
                try
                {

                var afiliaciones = await afiliacionSeguridadSocialService.GetAfiliacionSeguridadSocialByIdAsync(id);

                if (afiliaciones == null)
                    {
                        return NotFound(new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                        {
                            EsCorrecto = false,
                            Mensaje = "No se encontraron Afiliaciones.",
                            Valor = null,
                            CantRegistros = 0,
                        });
                    }

                    
                    return Ok(new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                    {
                        EsCorrecto = true,
                        Mensaje = "Afiliaciones obtenidas correctamente.",
                        Valor = afiliaciones,
                        CantRegistros = 1 ,
                    });
                }
                catch (Exception ex)
                {
                    await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpGet",
                        ruta: "/api/AfiliacionSeguridadSocial",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: "AfiliacionSeguridadSocial Controller"
                    );

                    return StatusCode(500, new ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "Ocurrió un error al obtener las afiliaciones a seguridad social.",
                        CantRegistros = 0
                    });
                }
            }
        }

    }
