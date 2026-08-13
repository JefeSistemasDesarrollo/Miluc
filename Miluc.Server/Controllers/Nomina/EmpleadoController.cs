using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.Models.Response;
using System.Text;

namespace Miluc.Server.Controllers.Nomina
{

    [ApiController]
    [Route("api/[controller]")]
#pragma warning disable S6961
    public class EmpleadoController(IEmpleadoService empleadoService, ILogService _log) : Controller

    {

        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<EmpleadoReaderDto>>>> GetEmpleadosAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
            
                var response = await empleadoService.GetEmpleadosAsync(filtro, page, cantidad);
                var empleados = response?.Valor;
            

                if (empleados == null || empleados.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<EmpleadoReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron empleados",
                        CantRegistros = 0,
                    });
                }

                return Ok(new ResponseAPI<List<EmpleadoReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = empleados,
                    Mensaje = "Se obtuvieron los empleados correctamente",
                    CantRegistros = response?.CantRegistros ?? empleados.Count,
                });
            }
            catch (Exception ex)
            {
                var errorReal = ObtenerMensajeDetallado(ex);

                await _log.GuardarErrorAsync(
                    message: errorReal,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: "/api/Empleado",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EmpleadoController");

                return StatusCode(500, new ResponseAPI<List<EmpleadoReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Ocurrió un error al consultar empleados: {errorReal}",
                    CantRegistros = 0
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<EmpleadoReaderDto>>> GetEmpleadoByIdAsync(int id)
        {
            try
            {
                var empleado = await empleadoService.GetEmpleadoByIdAsync(id);
                if (empleado == null)
                {
                    return NotFound(new ResponseAPI<EmpleadoReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "Empleado no encontrado",
                        CantRegistros = 0,
                    });
                }

                return Ok(new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = true,
                    Valor = empleado,
                    Mensaje = "Empleado obtenido correctamente",
                    CantRegistros = 1,
                });
            }
            catch (Exception ex)
            {
                var errorReal = ObtenerMensajeDetallado(ex);

                await _log.GuardarErrorAsync(
                    message: errorReal,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: $"/api/Empleado/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EmpleadoController");

                return StatusCode(500, new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Ocurrió un error al obtener el empleado: {errorReal}",
                    CantRegistros = 0
                });
            }
        }





        [HttpPost("crearEmpleado")]
        public async Task<ActionResult<ResponseAPI<EmpleadoReaderDto>>> CrearEmpleadoAsync([FromBody] EmpleadoCreateDto dto)
        {
            try
            {
                var empleado = await empleadoService.CreateEmpleadosAsync(dto);

                if (empleado == null)
                {
                    return BadRequest(new ResponseAPI<EmpleadoReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se pudo crear el empleado",
                        CantRegistros = 0,
                    });
                }

                return Ok(new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = true,
                    Valor = empleado,
                    Mensaje = "Empleado creado correctamente",
                    CantRegistros = 1,
                });
            }
            catch (Exception ex)
            {
                var errorReal = ObtenerMensajeDetallado(ex);

                await _log.GuardarErrorAsync(
                    message: errorReal,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/Empleado/crearEmpleado",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EmpleadoController");

                return StatusCode(500, new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Ocurrió un error al crear el empleado: {errorReal}",
                    CantRegistros = 0
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<EmpleadoReaderDto>>> UpdateEmpleadosAsync(int id, [FromBody] EmpleadoUpdateDto empleadoUpdateDto)
        {
            // Validar null
            if (empleadoUpdateDto == null)
            {
                return BadRequest(new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Debe enviar la información del empleado.",
                    CantRegistros = 0
                });
            }

            // Validar que el id de la URL coincida con el DTO
            if (id != empleadoUpdateDto.EmpleadoId)
            {
                return BadRequest(new ResponseAPI<EmpleadoReaderDto>
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
                        Errores = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
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
                var empleadoActualizado = await empleadoService.UpdateEmpleadosAsync(empleadoUpdateDto);

                if (empleadoActualizado == null)
                {
                    return NotFound(new ResponseAPI<EmpleadoReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontró el empleado.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = true,
                    Valor = empleadoActualizado,
                    Mensaje = "Empleado actualizado correctamente.",
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
                    metodo: "HttpPut",
                    ruta: $"/api/Empleado/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EmpleadoController"
                );

                return StatusCode(500, new ResponseAPI<EmpleadoReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Ocurrió un error al actualizar el empleado: {errorReal}",
                    CantRegistros = 0
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteEmpleadosAsync(int id)
        {
            try
            {
                var result = await empleadoService.DeleteEmpleadoAsync(id);

                if (!result)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "No se encontró el empleado.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Empleado eliminado correctamente.",
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
                    metodo: "HttpDelete",
                    ruta: $"/api/Empleado/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EmpleadoController"
                );

                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = $"Ocurrió un error al eliminar el empleado: {errorReal}",
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


