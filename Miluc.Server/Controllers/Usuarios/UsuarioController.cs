using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Usuarios
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UsuarioController(IUsuarioService _service, ILogService log) : Controller
    {
        [HttpGet("quien-soy")]
        public IActionResult QuienSoy()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new
            {
                Usuario = User.Identity?.Name,
                Autenticado = User.Identity?.IsAuthenticated,
                EsAdmin = User.IsInRole("Admin"),
                TodosLosClaims = claims
            });
        }

        // GET: api/usuarios?buscar=ypirajan&pagina=1&cantidad=20
        [HttpGet]
        [PermissionAuthorize("Usuario.View")]
        public async Task<ActionResult<ResponseAPI<List<UsuarioReadDto>>>> Get([FromQuery] string? buscar,
            [FromQuery] int pagina = 1,
            [FromQuery] int? cantidad = null)
        {
            try
            {
                var response = await _service.GetAllUsuariosAsync(buscar, pagina, cantidad);
               
                return Ok(new ResponseAPI<List<UsuarioReadDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de usuarios", // Total para el paginador de Blazor
                    Valor = response.Data,
                    CantRegistros = response.TotalRegistros,
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                         message: ex.Message,
                          StackTrace: ex.StackTrace,
                          usuario: User.Identity?.Name ?? "Sistema",
                          metodo: "HttpGet",
                          ruta: $"/api/Usuario​",
                          ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                          origen: $"UsuarioController");
                return StatusCode(500, new ResponseAPI<List<UsuarioReadDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        // GET: api/usuarios/5
        [HttpGet("{id}")]
        [PermissionAuthorize("Usuario.Detail")]
        public async Task<ActionResult<ResponseAPI<UsuarioReadDto>>> GetById(int id)
        {
            try
            {
                var user = await _service.GetByIdUsuarioAsync(id);
                if (user == null)
                {
                    return NotFound(new ResponseAPI<UsuarioReadDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Usuario no encontrado."
                    });
                }
                return Ok(new ResponseAPI<UsuarioReadDto> { EsCorrecto = true, Valor = user });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpGet",
                        ruta: $"/api/Usuario/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"UsuarioController");

                return StatusCode(500, new ResponseAPI<UsuarioReadDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener: {ex.Message}",
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        [HttpPost]
        [PermissionAuthorize("Usuario.Create")]
        public async Task<ActionResult<ResponseAPI<UsuarioReadDto>>> CreateUsuario([FromBody] UsuarioCreateDto dto)
        {
            var response = new ResponseAPI<UsuarioReadDto>();
            try
            {
                // 1. Procesar la Foto de Base64 a byte[]
                if (!string.IsNullOrEmpty(dto.FotoBase64))
                {
                    // Limpiar el encabezado data:image/jpeg;base64,
                    var base64Data = dto.FotoBase64.Contains(",") ? dto.FotoBase64.Split(',')[1] : dto.FotoBase64;
                    dto.Foto = Convert.FromBase64String(base64Data);
                }
                // 2. Asegurar que el IdTipoUsuario seleccionado se agregue a la lista que espera tu servicio
                if (dto.IdTipoUsuario > 0 && !dto.TiposUsuarioIds.Contains(dto.IdTipoUsuario))
                {
                    dto.TiposUsuarioIds.Add(dto.IdTipoUsuario);
                }

                var resultado = await _service.CreateUsuarioAsync(dto);

                return Ok(response.SuccessResponse(true, "Usuario creado exitosamente", resultado, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                 message: ex.Message,
                          StackTrace: ex.StackTrace,
                 usuario: User.Identity?.Name ?? "Sistema",
                 metodo: "HttpPost",
                 ruta: $"/api/Usuario",
                 ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                 origen: $"UsuarioController");
                return BadRequest(response.ErroresResponse(false, ex.Message, new List<string> { ex.InnerException?.Message ?? "" }));
            }
        }
        // PUT: api/usuarios
        [HttpPut]
        [PermissionAuthorize("Usuario.Update")]
        public async Task<ActionResult<ResponseAPI<UsuarioReadDto>>> Put([FromBody] UsuarioUpdateDto dto)
        {
            try
            {
                var result = await _service.UpdateUsuarioAsync(dto);
                if (result == null)
                {
                    return NotFound(new ResponseAPI<UsuarioReadDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Usuario no encontrado."
                    });
                }

                return Ok(new ResponseAPI<UsuarioReadDto>
                {
                    EsCorrecto = true,
                    Valor = result,
                    Mensaje = "Usuario actualizado correctamente."
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                   message: ex.Message,
                          StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPut",
                    ruta: $"/api/Usuario",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: $"UsuarioController");
                return BadRequest(new ResponseAPI<UsuarioReadDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        [PermissionAuthorize("Usuario.Delete")]
        public async Task<ActionResult<ResponseAPI<bool>>> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteUsuarioAsync(id);
                if (!success)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se pudo inactivar el usuario."
                    });
                }
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Usuario inactivado con éxito."
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpDelete",
                        ruta: $"/api/Usuario/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"UsuarioController");

                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }

    }
}
