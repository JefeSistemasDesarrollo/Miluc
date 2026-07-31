using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Encriptacion;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Server.Security;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.Security.Cryptography;
using System.Text;

namespace Miluc.Server.Controllers.Usuarios
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsuarioController(IUsuarioService _service, ILogService log) : Controller
    {

        [HttpGet("quien-soy")]
        public IActionResult QuienSoy()
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new
                    {
                        Mensaje = "El usuario no se encuentra autenticado."
                    });
                }

                var claims = User.Claims
                    .Select(c => new {c.Type,c.Value}).ToList();

            return Ok(new
            {
                    Usuario = User.Identity.Name,
                    Autenticado = true,
                EsAdmin = User.IsInRole("Admin"),
                TodosLosClaims = claims
            });
        }
            catch (Exception ex)
            {

                log.GuardarErrorAsync(
                 message: ex.Message,
                  StackTrace: ex.StackTrace,
                  usuario: User.Identity?.Name ?? "Sistema",
                  metodo: nameof(Get),
                  ruta: $"/api/Usuarios​",
                  ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                  origen: nameof(UsuarioController));


                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Mensaje = "Ocurrió un error interno al obtener la información del usuario."
                });
            }
        }

        //[HttpGet("quien-soy")]
        //public IActionResult QuienSoy()
        //{
        //    var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        //    return Ok(new
        //    {
        //        Usuario = User.Identity?.Name,
        //        Autenticado = User.Identity?.IsAuthenticated,
        //        EsAdmin = User.IsInRole("Admin"),
        //        TodosLosClaims = claims
        //    });
        //}

        // GET: api/usuarios?buscar=ypirajan&pagina=1&cantidad=20
        [HttpGet]
        [PermissionAuthorize("Usuario.View")]
        [ProducesResponseType(typeof(ResponseAPI<List<UsuarioReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<UsuarioReadDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<List<UsuarioReadDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<List<UsuarioReadDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAPI<List<UsuarioReadDto>>>> Get([FromQuery] string? buscar,
        [FromQuery] int? pagina = null,
            [FromQuery] int? cantidad = null)
        {
            try
            {
                var usuarios = await _service.GetAllUsuariosAsync(buscar, pagina, cantidad);
               
                if (usuarios.Data == null && !usuarios.Data.Any())
                {
                    return NotFound(new ResponseAPI<UsuarioReadDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron usuarios",
                        Valor = null,
                    });
                }

                return Ok(new ResponseAPI<List<UsuarioReadDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de usuarios", // Total para el paginador de Blazor
                    Valor = usuarios.Data,
                    CantRegistros = usuarios.TotalRegistros,
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<UsuarioReadDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<UsuarioReadDto>(),
                    Mensaje = $"No se encontraron usuarios Disponibles {ex.Message}",
                    Errores = [ex.Message]
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                         message: ex.Message,
                          StackTrace: ex.StackTrace,
                          usuario: User.Identity?.Name ?? "Sistema",
                       metodo: nameof(Get),
                       ruta: $"/api/Usuarios​",
                          ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                       origen: nameof(UsuarioController));

                return StatusCode(500, new ResponseAPI<List<UsuarioReadDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Ocurrio un error al obtener los usuarios",
                    Errores = [ex.Message]
                });
            }
        }
        // GET: api/usuarios/5
        [HttpGet("{id}")]
        [PermissionAuthorize("Usuario.Detail")]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAPI<UsuarioReadDto>>> GetById(int id)
        {
            try
            {
                var userId = await _service.GetByIdUsuarioAsync(id);

                if (userId == null)
                {
                    return NotFound(new ResponseAPI<UsuarioReadDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Usuario no encontrado."
                    });
                }
                return Ok(new ResponseAPI<UsuarioReadDto> { EsCorrecto = true, Valor = userId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseAPI<UsuarioReadDto>
                {
                    EsCorrecto = false,
                    Valor=null,
                    Mensaje = ex.Message,
                    Errores = [ex.Message]
                });

            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetById),
                        ruta: $"/api/Usuario/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(UsuarioController));

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
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status500InternalServerError)]
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

                if (resultado!=null)
                {
                    return NotFound(new ResponseAPI<UsuarioReadDto>
                    {
                        EsCorrecto=false,
                        Valor=null, 
                    });
                }

                return Ok(response.SuccessResponse(true, "Usuario creado exitosamente", resultado, 1));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseAPI<UsuarioReadDto>
                {
                    EsCorrecto = false,
                    Mensaje= ex.Message,
                    Valor= null
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseAPI<UsuarioReadDto>
                {
                    EsCorrecto = false,
                    Mensaje= ex.Message,
                    Valor= null
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                 message: ex.Message,
                          StackTrace: ex.StackTrace,
                 usuario: User.Identity?.Name ?? "Sistema",
                 metodo: nameof(CreateUsuario),
                 ruta: $"/api/Usuario",
                 ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                 origen: nameof(UsuarioController));

                return StatusCode(500, response.ErroresResponse(false, ex.Message, new List<string> { ex.InnerException?.Message ?? "" }));
            }
        }
        // PUT: api/usuarios
        [HttpPut]
        [PermissionAuthorize("Usuario.Update")]
       
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<UsuarioReadDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAPI<UsuarioReadDto>>> ActualizarUsuario([FromBody] UsuarioUpdateDto dto)
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
                    EsCorrecto = true,//2026-07-28T21:28:13.460Z
                    Valor = result, //2026-07-28T21:27:13.460Z
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
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteUsuario(int id)
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
                    Mensaje = "Usuario eliminado correctamente."
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(DeleteUsuario),
                        ruta: $"/api/Usuario/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(UsuarioController));

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

