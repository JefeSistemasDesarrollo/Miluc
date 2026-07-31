using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.DTOs.Nomina.ParentescoDto;
using Miluc.Shared.Models.Response;
using System.Drawing;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[Controller]")]
    //parentesco
    public class InfoFamiliarController(IParentescoService _parentescoService, IInfoFamiliarService _infoFamiliarService, ILogService _log) : Controller
    {
        [HttpGet("Parentesco")]
        public async Task<ActionResult<ResponseAPI<List<ParentescoReaderDto>>>> GetParentesco()
        {
            try
            {
                var parentescos = await _parentescoService.GetAllParentescosAsync();

                if (parentescos == null || parentescos.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<ParentescoReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron parentescos.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<ParentescoReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = parentescos,
                    Mensaje = "Parentescos obtenidos correctamente.",
                    CantRegistros = parentescos.Count
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: $"/api/Parentesco",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "ParentescoController");

                return StatusCode(500, new ResponseAPI<List<ParentescoReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error al obtener los parentescos: {ex.Message}",
                    CantRegistros = 0
                });
            }
        }
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<InfoFamiliarReaderDto>>>> GetInformacionFamiliar([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {

            try
            {
                var (infoFamiliar, totalRegistros) = await _infoFamiliarService.GetFamiliarListAsync(filtro, page, cantidad);
                if (infoFamiliar == null)
                {
                    return NotFound(new ResponseAPI<List<InfoFamiliarReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron registros de información familiar.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<InfoFamiliarReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = infoFamiliar,
                    Mensaje = "Información familiar obtenida correctamente.",
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
                     ruta: $"/api/InfoFamiliar",
                     ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                     origen: "InfoFamiliarController");
                return StatusCode(500, new ResponseAPI<List<InfoFamiliarReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error al obtener la información familiar: {ex.Message}",
                    CantRegistros = 0
                });


            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<List<InfoFamiliarReaderDto>>>> GetFamiliarByIdAsync(int id)
        {

            try
            {
                var infoFamiliar = await _infoFamiliarService.GetFamiliarByIdAsync(id);
                if (infoFamiliar == null)
                {
                    return NotFound(new ResponseAPI<List<InfoFamiliarReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron registros de información familiar.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<InfoFamiliarReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = infoFamiliar,
                    Mensaje = "Información familiar obtenida correctamente.",
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
                     ruta: $"/api/InfoFamiliar",
                     ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                     origen: "InfoFamiliarController");
                return StatusCode(500, new ResponseAPI<List<InfoFamiliarReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error al obtener la información familiar: {ex.Message}",
                    CantRegistros = 0
                });


            }

        }





        [HttpPost("CreateInformacionFamiliar")]
        public async Task<ActionResult<ResponseAPI<bool>>> CreateFamiliarAsync([FromBody] List<InfoFamiliarCreateDto> infoFamiliar)
        {
            // 1. MEJORA: Validación defensiva antes de tocar la base de datos
            if (infoFamiliar == null || !infoFamiliar.Any())
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "La lista de familiares enviada está vacía.",
                    CantRegistros = 0
                });
            }

           
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Uno o más campos de la lista de familiares no cumplen con las validaciones requeridas.",
                    CantRegistros = 0
                });
            }

            try
            {
                
                var resultado = await _infoFamiliarService.CreateFamiliarAsync(infoFamiliar);

                if (!resultado)
                {
                    return BadRequest(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "No fue posible registrar la información familiar en el sistema.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Información familiar registrada correctamente.",
                    CantRegistros = infoFamiliar.Count // Muestra cuántos insertó en total
                });
            }
            catch (Exception ex)
            {
               
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/InfoFamiliar/CreateInformacionFamiliar", 
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(InfoFamiliarController));

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = $"Error interno al registrar la información familiar: {ex.Message}",
                        CantRegistros = 0
                    });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<InfoFamiliarReaderDto>>> UpdateFamiliarAsync(int id, [FromBody] InfoFamiliarUpdateDto infoFamiliarDto)
        {
            // 1. Validación de objeto nulo
            if (infoFamiliarDto == null)
            {
                return BadRequest(new ResponseAPI<InfoFamiliarReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Se debe enviar la información del familiar.",
                    CantRegistros = 0
                });
            }

            // 2. Validación de coincidencia de ID
            if (id != infoFamiliarDto.InformacionFamiliarId)
            {
                return BadRequest(new ResponseAPI<InfoFamiliarReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "El ID enviado en la URL no coincide con el registro del familiar.",
                    CantRegistros = 0
                });
            }

            // 3. Validación de ModelState
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                return BadRequest(new ResponseAPI<InfoFamiliarReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Datos inválidos.",
                    Errores = errores,
                    CantRegistros = 0
                });
            }

            // 4. Ejecución del Servicio y Captura de Excepciones de Negocio
            try
            {
                // 4. Ejecución del servicio de aplicación
                var familiarActualizado = await _infoFamiliarService.UpdateFamiliarAsync(infoFamiliarDto);

                if (familiarActualizado == null)
                {
                    return NotFound(new ResponseAPI<InfoFamiliarReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontró el registro del familiar para actualizar.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<InfoFamiliarReaderDto>
                {
                    EsCorrecto = true,
                    Valor = familiarActualizado,
                    Mensaje = "Información familiar actualizada con éxito.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                //AQUÍ capturamos la excepción del servicio (como la validación de duplicados)
             
                return BadRequest(new ResponseAPI<InfoFamiliarReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = ex.Message, // Texto real: "Ya existe un familiar registrado..."
                    CantRegistros = 0
                });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteFamiliarAsync(int id)
        {
            try
            {
                var resultado = await _infoFamiliarService.DeleteFamiliarAsync(id);

                if (!resultado)
                {
                    return BadRequest(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "No se pudo eliminar la información familiar.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = resultado,
                    Mensaje = "Información familiar eliminada correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                     message: ex.Message,
                     StackTrace: ex.StackTrace,
                     usuario: User.Identity?.Name ?? "Sistema",
                     metodo: "HttpDelete",
                     ruta: $"/api/InfoFamiliar/DeleteFamiliar/{id}",
                     ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                     origen: "InfoFamiliarController");

                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = $"Error al eliminar la información familiar: {ex.Message}",
                    CantRegistros = 0
                });
            }
        }
    }
}
