using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunbacionDto;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacunasController(IVacunaService _service, IEsquemaVacunacionService _esquemaService, ILogService log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<VacunaReaderDto>>>> GetVacunas()
        {
            //var response = new ResponseAPI<List<VacunaReaderDto>>();
            try
            {
                var responseVacunas = await _service.GetVacunasAsync();


                if (responseVacunas == null || responseVacunas.Count == 0)
                {

                    return NotFound(new ResponseAPI<List<VacunaReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron vacunas.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<VacunaReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = responseVacunas,
                    Mensaje = "Vacunas obtenidas correctamente.",
                    CantRegistros = responseVacunas.Count
                }
                );

                //return Ok( response);
                // return Ok(vacunas);
            }
            catch (Exception ex)
            {

                await log.GuardarErrorAsync(
                        message: ex.Message,
                         StackTrace: ex.StackTrace,
                         usuario: User.Identity?.Name ?? "Sistema",
                         metodo: "HttpGet",
                         ruta: $"/api/Vacunas​",
                         ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                         origen: $"VacunasController");

                return StatusCode(500, new ResponseAPI<List<VacunaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });


            }
        }
        // Aquí puedes agregar más método de esquema de vacunacion get 
        [HttpGet("Esquemas")]
        public async Task<ActionResult<ResponseAPI<List<EsquemaVacunacionReaderDto>>>> GetEsquemasVacunacion([FromBody]string? filtro = null,[FromQuery]int page = 1, [FromQuery] int? cantidad = 1)
        {
            try
            {
                var (esqueamas,totalRegitros) = await _esquemaService.GetEsquemasVacunacionAsync(filtro, page, cantidad);
                if (esqueamas == null || esqueamas.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron esquemas de vacunación.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = esqueamas,
                    Mensaje = "Esquemas de vacunación obtenidos correctamente.",
                    CantRegistros = esqueamas.Count
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                        message: ex.Message,
                         StackTrace: ex.StackTrace,
                         usuario: User.Identity?.Name ?? "Sistema",
                         metodo: "HttpGet",
                         ruta: $"/api/Vacunas/Esquemas​",
                         ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                         origen: $"VacunasController");
                return StatusCode(500, new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }


        }
    }
}
