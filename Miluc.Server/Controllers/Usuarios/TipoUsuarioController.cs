using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Usuarios
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TipoUsuarioController(ITipoUsuario _tipoService, ILogService log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<TipoUsuarioReadDto>>>> GetAllTipoUsuario()
        {
            var response = new ResponseAPI<List<TipoUsuarioReadDto>>();
            try
            {
                var listaTipoUsuario = await _tipoService.GetAllTipoUsuarioAsync();
                if (listaTipoUsuario == null || !listaTipoUsuario.Any())
                {
                    return Ok(response.ErroresResponse(false, "No se encontraron tipos de usuarios", new List<string> { "La base de datos está vacía." }));
                }
                else 
                {
                    response.EsCorrecto=true;
                    response.Mensaje = "Tipo de usuario encontrados con exito";
                    response.Valor=listaTipoUsuario;
                    return Ok(response.SuccessResponse(true, "Tipos de usuario obtenidos correctamente", listaTipoUsuario, listaTipoUsuario.Count));

                }
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                        message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpGet",
                        ruta: $"/api/TipoUsuario​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"TipoUsuarioController");
                return StatusCode(500, response.ErroresResponse(false, "Error interno", new List<string> { ex.Message }));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseAPI<TipoUsuarioReadDto>>> GetByIdTipoUsuario(int id)
        {
            var response = new ResponseAPI<TipoUsuarioReadDto>();
            try
            {
                var tipoUsuario = await _tipoService.GetByIdTipoUsuarioAsync(id);
                if (tipoUsuario == null)
                {
                    return NotFound(response.ErroresResponse(false, "No se encontró el tipo de usuario", new List<string> { $"ID {id} no existe." }));
                }
                return Ok(response.SuccessResponse(true, "Tipo de usuario obtenido correctamente", tipoUsuario, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                        message: ex.Message,
                          StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpPut",
                        ruta: $"/api/TipoUsuario/{id}​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"TipoUsuarioController");

                return StatusCode(500, response.ErroresResponse(false, "Error interno", new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseAPI<bool>>> CreateTipoUsuario([FromBody] TipoUsuarioCreateDto tipoUsuarioCreateDto)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                // Como el servicio devuelve Task<bool>
                var resultado = await _tipoService.CreateTipoUsuarioAsync(tipoUsuarioCreateDto);

                if (!resultado)
                {
                    return Ok(response.ErroresResponse(false, "No se pudo crear el tipo de usuario", new List<string> { "La base de datos no confirmó el cambio." }));
                }

                return Ok(response.SuccessResponse(true, "Tipo de usuario creado exitosamente", true, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                           message: ex.Message,
                          StackTrace: ex.StackTrace,
                            usuario: User.Identity?.Name ?? "Sistema",
                            metodo: "HttpPost",
                            ruta: $"/api/TipoUsuario",
                            ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                            origen: $"TipoUsuarioController");
              
                return BadRequest(response.ErroresResponse(false, "Error al crear tipo de usuario", new List<string> { ex.Message }));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResponseAPI<bool>>> UpdateTipoUsuario([FromBody] TipoUsuarioUpdateDto tipoUsuarioUpdateDto)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                var resultado = await _tipoService.UpdateTipoUsuarioAsync(tipoUsuarioUpdateDto);

                if (resultado == null)
                {
                    return NotFound(response.ErroresResponse(false, "No se pudo actualizar", new List<string> { $"ID {tipoUsuarioUpdateDto.IdTipoUsuario} no encontrado." }));
                }

                return Ok(response.SuccessResponse(true, "Tipo de usuario actualizado correctamente", resultado.Value, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                           message: ex.Message,
                          StackTrace: ex.StackTrace,
                           usuario: User.Identity?.Name ?? "Sistema",
                           metodo: "HttpPut",
                           ruta: $"/api/TipoUsuario",
                           ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                           origen: $"TipoUsuarioController");
                return BadRequest(response.ErroresResponse(false, "Error al actualizar", new List<string> { ex.Message }));
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteTipoUsuario(int id)
        {
            var response = new ResponseAPI<bool>();
            try
            {
                var resultado = await _tipoService.DeleteTipoUserAsync(id);
                if (!resultado)
                {
                    return NotFound(response.ErroresResponse(false, "No se pudo eliminar", new List<string> { $"ID {id} no existe." }));
                }
                return Ok(response.SuccessResponse(true, "Tipo de usuario eliminado correctamente", true, 1));
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                     message: ex.Message,
                          StackTrace: ex.StackTrace,
                       usuario: User.Identity?.Name ?? "Sistema",
                       metodo: "HttpDelete",
                       ruta: $"/api/TipoUsuario/{id}",
                       ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                       origen: $"TipoUsuarioController");
                // Captura el error de integridad (usuarios asociados)
                return BadRequest(response.ErroresResponse(false, "Conflicto de eliminación", new List<string> { ex.Message }));
            }
        }


    }
}
