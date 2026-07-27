using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.CiudadMM;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[controller]")]
    public class CiudadMediosMagneticosController(ISapCiudadMMService sapCiudadMMService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAllBPCO_MUAsync([FromQuery] string? buscar = null,
         [FromQuery] int pagina = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (data, totalRegistros) = await sapCiudadMMService.GetAllBPCO_MUAsync(buscar, pagina, cantidad);
                return Ok(new { Data = data, TotalRegistros = totalRegistros });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los datos de BPCO_MU: {ex.Message}");
            }
        }

    }
}
