using Microsoft.AspNetCore.Mvc;

using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacunasController(IVacunaService _service,  ILogService _log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<VacunaReaderDto>>>> GetVacunaAsync()
        {
            //var response = new ResponseAPI<List<VacunaReaderDto>>();
            try
            {
                var responseVacunas = await _service.GetVacunaAsync();


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

                await _log.GuardarErrorAsync(
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
            

            }

        }

    


    
    

