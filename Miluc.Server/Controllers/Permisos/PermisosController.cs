using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Permisos;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Permisos
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class PermisosController(ILogService log, IPermisosService _permisoService) : Controller
    {
        //
        [HttpGet]
        [PermissionAuthorize("Permiso.View")]
        public async Task<ActionResult<ResponseAPI<List<PermisosReadDto>>>> GetPermisosAsync(
        [FromQuery] string? busqueda,
        [FromQuery] int pagina = 1,
        [FromQuery] int? cantidad = null)
        {
            var responseAPI = new ResponseAPI<List<PermisosReadDto>>();
            try
            {
                var (data, totalRegistros) = await _permisoService.GetAllPermisosAsyncPage(busqueda, pagina, cantidad);

            
                return Ok(new ResponseAPI<List<PermisosReadDto>>
                {
                    EsCorrecto = true,
                    Valor = data,
                    CantRegistros = totalRegistros
                });
            }
            catch (Exception ex)
            {
                //await log.GuardarErrorAsync(ex);

                await log.GuardarErrorAsync(
                 message: ex.Message,
                 StackTrace: ex.StackTrace,
                usuario: User.Identity?.Name ?? "Sistema",
                metodo: "GET",
                ruta: "/api/Permisos",
                ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                origen: "PermisosController");

                return StatusCode(500,
                    responseAPI.ErroresResponse(
                        false,
                        "Error interno",
                        new List<string> { ex.Message }
                    )
                );
            }
        }
        [HttpPost]
        [PermissionAuthorize("Permiso.Create")]
        public async Task<ActionResult<ResponseAPI<bool>>> CreatePermisoAsync([FromBody] PermisosCreateDto createDto)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                if (createDto == null)
                {
                    return BadRequest();
                }
                if (createDto.RolesIds.Count > 0 && !createDto.RolesIds.Contains(createDto.IdRol))
                {
                    createDto.RolesIds.Add(createDto.IdRol);
                }
                var permisoresult = await _permisoService.CreatePermisosAsync(createDto);
                return Ok(response.SuccessResponse(true, "Permiso creado existosamente", permisoresult, 1));
            }
            catch (Exception ex)
            {

                await log.GuardarErrorAsync(
             message: ex.Message,
            StackTrace: ex.StackTrace,
            usuario: User.Identity?.Name ?? "Sistema",
            metodo: "HttpPost",
            ruta: "/api/Permisos",
            ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
            origen: $"PermisosController");
                return BadRequest(response.ErroresResponse(false, ex.Message, new List<string> { ex.InnerException?.Message ?? "" }));
            }
        }
        [HttpGet("{id:int}")]
        [PermissionAuthorize("Permiso.Detail")]
        public async Task<ActionResult<ResponseAPI<PermisosReadDto>>> GetByIdPermiso(int id)
        {
            ResponseAPI<PermisosReadDto> responseAPI = new();
            try
            {
                var permiso = await _permisoService.GetByIdPermisoAsync(id);

                if (permiso == null)
                {
                    responseAPI.EsCorrecto = false;
                    responseAPI.Valor = null;
                    responseAPI.Mensaje = $"Error al consultar el permiso {id}";

                    return NotFound(responseAPI);
                }
                return Ok(responseAPI.SuccessResponse(true, "Consultado correctamente", permiso, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                        message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpGetid",
                        ruta: $"/api/Permisos/{id}​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"PermisosController");

                return StatusCode(500,
                   responseAPI.ErroresResponse(
                       false,
                       "Error interno",
                       new List<string> { ex.Message }
                   )
               );

            }

        }
        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        [PermissionAuthorize("Permiso.Delete")]
        public async Task<ActionResult<ResponseAPI<bool>>> Delete(int id)
        {
            try
            {
                var success = await _permisoService.DeletePermisoAsync(id);
    

                if (!success)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se pudo inactivar el permiso."
                    });
                }
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "permiso inactivado con éxito."
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                        message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpDelete",
                        ruta: $"/api/Permisos/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"PermisosController");
                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut]
        [PermissionAuthorize("Permiso.Update")]
        public async Task<ActionResult<ResponseAPI<bool>>> ActualizarPemriso([FromBody] PermisosUpdateDto dto)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                var result = await _permisoService.UpdatePermisosAsync(dto);

                //aca result !=null
                if (result == false)
                {
                    response.EsCorrecto = false;
                    response.Mensaje = "Permiso no encontrado";
                    return NotFound(response);
                }
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = result,
                    Mensaje = "Permiso actualizado correctamente Correctamente"
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                   message: ex.Message,
                          StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPUT",
                    ruta: "/api/Permisos",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: $"PermisosController");
                return BadRequest(new ResponseAPI<UsuarioReadDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
    }
}
