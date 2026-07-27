using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.Oslp;
using Miluc.Shared.DTOs.Sap.Vendedor;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{

    [ApiController]
    [Route("api/[Controller]")]
    public class OslpController(ISapOslpService _oslpService, ILogService log) : Controller
    {

        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<SapVendedorReaderDto>>>> GetListOslpAsync()
        {
            try
            {
                var response = new ResponseAPI<List<SapVendedorReaderDto>>();

                var listaVendedores = await _oslpService.GetListOslpAsync();
                if (response == null)
                {
                    return Ok(response.ErroresResponse(false, "No se encontraron vendedores", new List<string> { " Error al obtener los vendores o no hay" }));
                }

                return Ok(new ResponseAPI<List<SapVendedorReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de clientes obtenida correctamente",
                    Valor = listaVendedores
                });

            }
            catch (Exception ex)
            {

                await log.GuardarErrorAsync(
                        message: ex.Message,
                         StackTrace: ex.StackTrace,
                         usuario: User.Identity?.Name ?? "Sistema",
                         metodo: "HttpGet",
                         ruta: $"/api/oslp​",
                         ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                         origen: $"OslpController");

                return StatusCode(500, new ResponseAPI<List<SapVendedorReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });


            }


        }
    }
}
