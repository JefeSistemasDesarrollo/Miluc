using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EstadoCivilDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoCivilController(IEstadoCivilService estadoCivilService, ILogService _log) : Controller
    {

        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<EstadoCivilReaderDto>>>> GetAllAsync()
        {
            try
            {
                var estadosCiviles = await estadoCivilService.GetAllEstadosCivilesAsync();


                if (estadosCiviles == null || estadosCiviles.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<EstadoCivilReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron estados civiles.",
                        CantRegistros = 0
                    });
                }


                return Ok(new ResponseAPI<List<EstadoCivilReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = estadosCiviles,
                    Mensaje = "Estados civiles obtenidos correctamente.",
                    CantRegistros = estadosCiviles.Count
                }
                );



            }
            catch (Exception ex)
            {


                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: $"/api/EstadoCivil",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "EstadoCivilController");


                return StatusCode(500, new ResponseAPI<List<EstadoCivilReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = $"Error al obtener los estados civiles: {ex.Message}",
                    CantRegistros = 0
                });

            }
        }
    }
}
