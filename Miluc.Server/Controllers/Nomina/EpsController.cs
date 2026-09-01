using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Reader;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.Models.Response;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Miluc.Server.Controllers.Nomina
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpsController(IEpsService epsService, ILogService _log) : Controller
    {




        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<EpsReaderDto>>>> GetEpsAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var eps = await epsService.GetEpsAsync(filtro, page, cantidad);
                if (eps.Data == null || eps.Data.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<EpsReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron EPS",
                        CantRegistros = 0,
                    });
                }



                return Ok(new ResponseAPI<List<EpsReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = eps.Data,
                    Mensaje = "Se obtuvieron las EPS correctamente",
                    CantRegistros = eps.TotalRegistros,
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: "/api/Eps",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EpsController");

                return StatusCode(500, new ResponseAPI<List<EpsReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "",
                    CantRegistros = 0
                });
            }
        }
        [HttpPost("CrearEps")]
        public async Task<ActionResult<ResponseAPI<EpsReaderDto>>> CreateEpsAsync(EpsCreateDto epsCreateDto)
        {
            if (epsCreateDto == null || string.IsNullOrWhiteSpace(epsCreateDto.Nombre))
                return BadRequest(new ResponseAPI<EpsReaderDto> { EsCorrecto = false, Mensaje = "Los datos no pueden ser nulos." });

            try
            {
                var resultado = await epsService.CreateEpsAsync(epsCreateDto);
                return Ok(new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = true,
                    Valor = resultado,
                    Mensaje = "EPS creada exitosamente.",
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
                    ruta: "/api/Eps/CrearEps",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EpsController"
                );

                // Devolvemos ex.Message para que el Admin sepa por qué falló (ej: "Nombre duplicado")
                return StatusCode(500, new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = ex.Message,
                    CantRegistros = 0
                });
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<EpsReaderDto>>> GetByEpsAsync(int id)


        {
            try
            {
                var eps = await epsService.GetByEpsAsync(id);
                if (eps == null)
                {
                    return NotFound(new ResponseAPI<EpsReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "eps no encontrado",
                        CantRegistros = 0,
                    });
                }
                return Ok(new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = true,
                    Valor = eps,
                    Mensaje = "eps obtenido correctamente",
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
                    ruta: $"/api/Eps/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EPSController");
                return StatusCode(500, new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener el eps.",
                    CantRegistros = 0
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<EpsReaderDto>>> UpdateEpsAsync(int id, [FromBody] UpdateEpsDto updateEps)
        {
            // 1. Validaciones previas en un solo bloque 
            if (updateEps == null)
                return BadRequest(ErrorResponse("Se debe enviar la información de la EPS."));

            if (id != updateEps.EpsId)
                return BadRequest(ErrorResponse("El ID enviado en la URL no coincide con la EPS."));

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ErrorResponse("Datos inválidos.", errores));
            }

            try
            {
                var epsActualizada = await epsService.UpdateEpsAsync(updateEps);

                return epsActualizada is null
                    ? NotFound(ErrorResponse("No se encontró la EPS para actualizar."))
                    : Ok(new ResponseAPI<EpsReaderDto>
                    {
                        EsCorrecto = true,
                        Valor = epsActualizada,
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
                    ruta: $"/api/Eps/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(EpsController)
                );

                return BadRequest(ErrorResponse(ex.Message));
            }
        }


        private static ResponseAPI<EpsReaderDto> ErrorResponse(string mensaje, List<string>? errores = null) => new()
        {
            EsCorrecto = false,
            Valor = null,
            Mensaje = mensaje,
            Errores = errores,
            CantRegistros = 0
        };


        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteEpsAsync(int id)
        {
            try
            {
                var result = await epsService.DeleteEpsAsync(id);

                if (!result)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "No se encontró el eps.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Esp eliminado correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.ToString(),
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "DeleteEpsAsync", // Es recomendable poner el nombre real del método
                    ruta: $"/api/Eps/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EpsController"
                );

                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = $" {ex.Message}",
                    CantRegistros = 0
                });
            }
        }
    }
}









