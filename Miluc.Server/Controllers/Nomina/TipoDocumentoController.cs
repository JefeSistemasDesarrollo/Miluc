using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.TipoDocumento;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina

{

    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocumentoController(ITipoDocumentoService tipoDocumentoService, ILogService _log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<TipoDocumentoReaderDto>>>> GetAllAsync()
        {
            try
            {
                var tipoDocumentos = await tipoDocumentoService.GetAllTipoDocumento();
                if (tipoDocumentos == null || tipoDocumentos.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<TipoDocumentoReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron tipos de documento.",
                        CantRegistros = 0
                    }); 
                }


                return Ok(new ResponseAPI<List<TipoDocumentoReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = tipoDocumentos,
                    Mensaje = "Tipos de documento obtenidos correctamente.",
                    CantRegistros = tipoDocumentos.Count
                });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                 message: ex.Message,
                 StackTrace: ex.StackTrace,
                 usuario: User.Identity?.Name ?? "Sistema",
                 metodo: "HttpGet",
                 ruta: $"/api/tipoDocumento",
                 ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                 origen: "TipoDocumentoController");


                return StatusCode(500, new ResponseAPI<List<TipoDocumentoReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error al obtener los tipos de documento: {ex.Message}",
                    CantRegistros = 0
                });
            }
        }
    }
}


