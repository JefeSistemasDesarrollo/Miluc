using Azure;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.Itm1Sap;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Cliente;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{

    [ApiController]
    [Route("api/[Controller]")]
    public class Itm1Controller(ISapItm1Service itm1Service, ILogService log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<Response<List<ITM1>>>> GetItm1Async()
        {
            try
            {
                var listItm1 = await itm1Service.GetListItm1Async();

                return Ok(new ResponseAPI<List<ITM1>>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de 1ITM",
                    Valor = listItm1,
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                                        message: ex.Message,
                                         StackTrace: ex.StackTrace,
                                         usuario: User.Identity?.Name ?? "Sistema",
                                         metodo: "HttpGet",
                                         ruta: $"/api/Itm1​",
                                         ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                                         origen: $"Itm1Controller");
                return StatusCode(500, new ResponseAPI<List<SapClienteReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }


    }
}
