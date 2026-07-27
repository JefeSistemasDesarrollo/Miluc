using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController, Route("api/[controller]")]
    public class CajaCompensacionController(ICajaCompensacionService cajaService, ILogService _log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<CajaCompensacionReaderDto>>>> GetCajaCompensacionAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                
                var (caja, totalRegistrosBD) = await cajaService.GetCajaCompensacionAsync(filtro, page, cantidad);

                if (caja == null || !caja.Any())
                {
                    return NotFound(new ResponseAPI<List<CajaCompensacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron registros de Caja Compensacion",
                        Valor = null,
                        CantRegistros = 0,
                    });
                }

                return Ok(new ResponseAPI<List<CajaCompensacionReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = caja,
                    Mensaje = "Caja Compensacion obtenidas exitosamente.",
                    CantRegistros = totalRegistrosBD 
                });
            }
            catch (Exception ex)
            {
                
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: "/api/CajaCompensacion",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "CajaCompensacion Controller"
                );

               
                return StatusCode(500, new ResponseAPI<List<CajaCompensacionReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener CajaCompensacion.",
                    CantRegistros = 0
                });
            }
        }
        [HttpPost("CrearCaja")]
        public async Task<ActionResult<ResponseAPI<CajaCompensacionReaderDto>>> CreateCajaAsync(CajaCreateDto cajaCreateDto)
        {
            if (cajaCreateDto == null || string.IsNullOrEmpty(cajaCreateDto.Nombre))

                return BadRequest(new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Los campos Nombre y Codigo son obligatorios.",

                });
            try
            {
                var nuevaCaja = await cajaService.CreateCajaAsync(cajaCreateDto);
                return Ok(new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = true,
                    Mensaje = "Caja Compensacion creada exitosamente.",
                    Valor = nuevaCaja,
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
                    ruta: "/api/CajaCompensacion/CrearCaja",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "CajaCompensacion Controller"
                );
                return StatusCode(500, new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null, 
                    Mensaje =ex.Message,
                    CantRegistros = 0
                });



            }
        }
    }
}





