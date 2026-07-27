using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Roles;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Roles;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Roles
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class RolesController(IRolService _rolService, ILogService log) : Controller
    {

        [HttpGet]
        [PermissionAuthorize("Rol.View")]
        public async Task<ActionResult<ResponseAPI<List<RolReadDto>>>> GetRoles()
        {
            var response = new ResponseAPI<List<RolReadDto>>();
            try
            {
                var listaroles = await _rolService.GetAllRolesAsync();

                if (listaroles == null || listaroles.Count == 0)
                {
                    return Ok(response.ErroresResponse(false, "No se encontraron roles", new List<string> { "La base de datos de roles está vacía." }));
                }

                return Ok(response.SuccessResponse(true, "Lista de roles obtenida", listaroles, listaroles.Count));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpGet",
                        ruta: "/api/roles",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"RolesController.GET");

                return StatusCode(500, response.ErroresResponse(false, "Error interno", new List<string> { ex.Message }));
            }
        }

        [HttpGet("{id:int}")]
        [PermissionAuthorize("Rol.Detail")]
        public async Task<ActionResult<ResponseAPI<RolReadDto>>> GetRolById(int id)
        {
            var response = new ResponseAPI<RolReadDto>();
            try
            {
                var rol = await _rolService.GetByIdRolesAsync(id);
                if (rol == null)
                {
                    return Ok(response.ErroresResponse(false, "No se encontró el rol", new List<string> { $"ID {id} no existente." }));
                }
                return Ok(response.SuccessResponse(true, "Rol obtenido", rol, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                        StackTrace: ex.StackTrace,
                       usuario: User.Identity?.Name ?? "Sistema",
                       metodo: "HttpGetID",
                       ruta: "/api/roles",
                       ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                       origen: $"RolesController.GetId");
                return StatusCode(500, response.ErroresResponse(false, "Error interno", new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        [PermissionAuthorize("Rol.Create")]
        public async Task<ActionResult<ResponseAPI<bool>>> CreateRol([FromBody] RolCreateDto rolCreateDto)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                // Tu servicio devuelve bool, no el objeto creado
                var exito = await _rolService.CreateRolAsync(rolCreateDto);

                if (!exito)
                {
                    return Ok(response.ErroresResponse(false, "No se pudo crear el rol", new List<string> { "El nombre del rol ya existe o los datos son inválidos." }));
                }

                return Ok(response.SuccessResponse(true, "Rol creado exitosamente", true, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                   message: ex.Message,
                        StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPost",
                    ruta: "/api/roles",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: $"RolesController.Pos");
                return StatusCode(500, response.ErroresResponse(false, "Error interno", new List<string> { ex.Message }));
            }
        }

        [HttpPut] // Corregido de Post a Put
        [PermissionAuthorize("Rol.Update")]
        public async Task<ActionResult<ResponseAPI<bool>>> UpdateRol([FromBody] RolUpdateDto rolUpdateDto)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                var exito = await _rolService.UpdateRolAsync(rolUpdateDto);
                if (!exito)
                {
                    return Ok(response.ErroresResponse(false, "No se pudo actualizar el rol", new List<string> { "ID no encontrado o nombre duplicado." }));
                }
                return Ok(response.SuccessResponse(true, "Rol actualizado exitosamente", true, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                          message: ex.Message, StackTrace: ex.StackTrace,
                          usuario: User.Identity?.Name ?? "Sistema",
                          metodo: "HttpPut",
                          ruta: "/api/roles",
                          ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                          origen: $"RolesController.HttpPut");
                return StatusCode(500, response.ErroresResponse(false, "Error interno", new List<string> { ex.Message }));
            }
        }

        [HttpDelete("{id:int}")]
        [PermissionAuthorize("Rol.Delete")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteRol(int id)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                var eliminado = await _rolService.DeleteRolAsync(id);
                if (!eliminado)
                {
                    return Ok(response.ErroresResponse(false, "No se pudo eliminar el rol", new List<string> { "El rol tiene usuarios asociados o no existe." }));
                }
                return Ok(response.SuccessResponse(true, "Rol eliminado exitosamente", true, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                      message: ex.Message,
                        StackTrace: ex.StackTrace,
                     usuario: User.Identity?.Name ?? "Sistema",
                     metodo: "HttpDelete",
                     ruta: $"/api/Roles/{id}",
                     ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                     origen: $"RolesController");
                return StatusCode(500, response.ErroresResponse(false, "Error interno", new List<string> { ex.Message }));
            }
        }
    }
}
