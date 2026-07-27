using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Shared.DTOs.LogDots;

namespace Miluc.Server.Controllers.LoggerController
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController(ILogService _logService) : Controller
    {
        [HttpPost("registrar-error")]
        public async Task<IActionResult> RegistrarError([FromBody] ErrorLogRequest request)
        {
            try
            {
                await _logService.GuardarErrorAsync(
               message: request.Mensaje,
               StackTrace: request.StackTrace,
               usuario: request.Usuario,
               metodo: request.Metodo,
               ruta: request.Ruta,
               ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               origen: $"BlazorClient:"
           );
                return Ok();
            }
            catch (Exception ex) 
            {
                await _logService.GuardarErrorAsync(
                 message: ex.Message,
                 StackTrace: ex.StackTrace,
                usuario: User.Identity?.Name ?? "Sistema",
                metodo: "POS",
                ruta: "/registrar-error",
                ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                origen: "LogsController");
            }
            return StatusCode(500);
        }
    }
}
