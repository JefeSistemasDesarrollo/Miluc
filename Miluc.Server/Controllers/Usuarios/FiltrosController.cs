using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FiltrosController(IFiltrosUsuarios _filtrosService, ILogService _log ) : Controller
    {
        [HttpGet("Filtro")]
        [PermissionAuthorize("Filtro.View")]
        public async Task<ActionResult<ResponseAPI<List<AsignarFiltrosReaderDto>>>> GetFiltrosAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (filtros, totalRegistros) = await _filtrosService.GetFiltrosAsync(filtro, page, cantidad);
                //if (filtros. == null || filtros.Count == 0)
                //{
                //    return NotFound(new ResponseAPI<List<AsignarFiltrosReaderDto>>
                //    {
                //        EsCorrecto = false,
                //        Mensaje = "No se encontraron filtros.",
                //        Valor = null,
                //        CantRegistros = 0
                //    });
                //}

                return Ok(new ResponseAPI<List<AsignarFiltrosReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Filtros obtenidos exitosamente.",
                    Valor = filtros,
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
                    ruta: "/api/Filtros",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "Filtros Controller"
                );

                return StatusCode(500, new ResponseAPI<List<AsignarFiltrosReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener filtros.",
                    CantRegistros = 0
                });
            }
        }
        [HttpPost("CrearFiltro")]
        public async Task<ActionResult<ResponseAPI<bool>>> CreateFiltrosAsync(AsignarFiltrosCreateDto filtroCreateDto)
        {
            // 1. Validación de integridad: Verifica que el objeto y sus campos esenciales no estén vacíos.
            if (filtroCreateDto == null || string.IsNullOrWhiteSpace(filtroCreateDto.NombreModulo))
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "El nombre del módulo es obligatorio y no puede estar vacío.",
                });

            try
            {
                // Invocación del servicio: Delega la lógica de negocio y persistencia.
                var nuevoFiltro = await _filtrosService.CreateFiltrosAsync(filtroCreateDto);

                // Respuesta exitosa: Retorna el DTO procesado con su nuevo ID.
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Mensaje = "Filtro creado exitosamente.",
                    Valor = true,
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
                    ruta: "/api/Filtros/CrearFiltro",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "Filtros Controller"
                );

                // Control de excepciones: Retorna el mensaje de error específico al usuario.
                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = ex.Message,
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<AsignarFiltrosReaderDto>>> GetByIdFiltrosAsync(int id)
        {
            try
            {
                var filtro = await _filtrosService.GetByIdFiltrosAsync(id);
                if (filtro == null)
                {
                    return NotFound(new ResponseAPI<AsignarFiltrosReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "Filtro no encontrado",
                        CantRegistros = 0,
                    });
                }
                return Ok(new ResponseAPI<AsignarFiltrosReaderDto>
                {
                    EsCorrecto = true,
                    Valor = filtro,
                    Mensaje = "Filtro obtenido correctamente",
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
                    ruta: $"/api/Filtros/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "FiltrosController");
                return StatusCode(500, new ResponseAPI<AsignarFiltrosReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener el Filtro.",
                    CantRegistros = 0
                });
            }
        }


        [HttpPut]
        public async Task<ActionResult<ResponseAPI<bool>>> UpdateFiltrosAsync( [FromBody] AsignarFiltroUpdateDto updateFiltro)
        {
            // 1. Validaciones previas en un solo bloque 
            if (updateFiltro == null)
                return BadRequest(ErrorResponse("Se debe enviar la información del Filtro."));

            //if (id != updateFiltro.AsignarFiltrosId)
            //    return BadRequest(ErrorResponse("El ID enviado en la URL no coincide con el Filtro."));

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ErrorResponse("Datos inválidos.", errores));
            }

            try
            {
                var filtroActualizado = await _filtrosService.UpdateFiltrosAsync(updateFiltro);


                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Filtro actualizado correctamente.",
                    CantRegistros = 1
                });

                    //? NotFound(ErrorResponse("No se encontró el Filtro para actualizar."))
                    //: Ok(new ResponseAPI<bool>
                    //{
                    //    EsCorrecto = true,
                    //    Valor = true,
                    //    Mensaje = "Filtro actualizado correctamente.",
                    //    CantRegistros = 1
                    //});
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPut",
                    ruta: $"/api/filtros",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(FiltrosController)
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
                var result = await _filtrosService.DeleteFiltrosAsync(id);

                if (!result)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "No se encontró la filtro.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Filtro eliminado correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.ToString(),
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "DeleteFiltroAsync", // Es recomendable poner el nombre real del método
                    ruta: $"/api/Filtros/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "FiltrosController"
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
        [HttpGet("Permiso")]
        public async Task<ActionResult<ResponseAPI<List<PermisosReadDto>>>> GetModulos([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {

            try
            {
                var (Data, TotalRegistros) = await _filtrosService.GetAllPermisosModuloAsyncPage(filtro, page, cantidad);

                if (Data == null || !Data.Any())
                {

                    return NotFound(new ResponseAPI<List<PermisosReadDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron módulos.",
                        CantRegistros = 0
                    });   
                }

                return Ok(new ResponseAPI<List<PermisosReadDto>>
                {
                    EsCorrecto = true,
                    Valor = Data,
                    Mensaje = "filro eliminado correctamente.",
                    CantRegistros = TotalRegistros
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.ToString(),
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "DeleteFiltroAsync", // Es recomendable poner el nombre real del método
                    ruta: $"/api/Filtros/",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "FiltrosController"
                );

                return StatusCode(500, new ResponseAPI<List<PermisosReadDto>>
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
