using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.ConexionSap;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ConfiguracionController(IConexionServiceLayer _serviceLayer) : Controller
    {
        [HttpPut("ActualizarConfiguraciones")]
        public async Task<IActionResult> ActualizarPasswordSAP([FromBody] string password)
        {
            var respuesta = await _serviceLayer.ActualizarConfiguracionSAPAsync(password);
            return Ok(respuesta);
        }
    }
}
