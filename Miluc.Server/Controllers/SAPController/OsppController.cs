using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.Ospp;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[controller]")]
    public class OsppController(ISapOsppService _sapOsppService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetPreciosEspeciales()
        {
            try
            {
                // Lógica para obtener los precios especiales desde el servicio
                var preciosEspeciales = await _sapOsppService.GetPreciosEspecialesAsync();
                return Ok(preciosEspeciales);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error al obtener precios especiales: {ex.Message}");
            }
        }
    }
}
