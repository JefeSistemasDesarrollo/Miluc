using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatrizSociodemograficaController(IMatrizSociodemograficaService matrizSociodemograficaService, ILogService _log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<MatrizSociodemograficaReaderDto>>>> GetMatrizSociodemograficaAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (matrizSociodemografica, totalRegistros) = await matrizSociodemograficaService.GetMatrizSocioDemograficasAsync(filtro, page, cantidad);
                if (matrizSociodemografica == null || matrizSociodemografica.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<MatrizSociodemograficaReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron registros de la matriz sociodemográfica",
                        CantRegistros = 0,
                    });
                }
                return Ok(new ResponseAPI<List<MatrizSociodemograficaReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = matrizSociodemografica,
                    Mensaje = "Se obtuvieron los registros de la matriz sociodemográfica correctamente",
                    CantRegistros = totalRegistros,
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: "/api/MatrizSociodemografica",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "MatrizSociodemograficaController");
                return StatusCode(500, new
                {
                    EsCorrecto = false,
                    Valor = (object?)null,
                    Mensaje = "Ocurrió un error al obtener los registros de la matriz sociodemográfica.",
                    CantRegistros = 0
                });
            }
        }

    }
}