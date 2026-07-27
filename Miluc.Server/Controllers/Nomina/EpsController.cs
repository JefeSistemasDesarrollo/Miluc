using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.Models.Response;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Miluc.Server.Controllers.Nomina
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpsController(IEpsService epsService, ILogService _log) : Controller
    {



        
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<EpsReaderDto> >>> GetEpsAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
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
                    ruta: "/api/Empleado",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EmpleadoController");

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
    }
}


            