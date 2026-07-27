using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.Obpp;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Rutas;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ObppController (ISapObppService _service, ILogService log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<SapObppDto>>>> GetObpp()
        {
            try
            {
                var obpplist = await _service.GetObppsAsync();
                return Ok(new ResponseAPI<List<SapObppDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de obpp",
                    Valor = obpplist,
                    CantRegistros=obpplist.Count
                });

            }
            catch (Exception ex)
            {

                await log.GuardarErrorAsync(
                                        message: ex.Message,
                                         StackTrace: ex.StackTrace,
                                         usuario: User.Identity?.Name ?? "Sistema",
                                         metodo: nameof(GetObpp),
                                         ruta: $"/api/ObppController​",
                                         ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                                         origen: $"Itm1Controller");
                return StatusCode(500, new ResponseAPI<List<OBPP>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
    }
}
