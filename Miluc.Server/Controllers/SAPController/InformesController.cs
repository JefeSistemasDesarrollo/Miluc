using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.Informes.InformeComercial;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    //[Authorize]
    public class InformesController(ISapFrecuenciaVentasService _service) : Controller
    {
        [HttpGet("FrecuenciaPorVendedor/Excel")]
        public async Task<IActionResult> FrecuenciaPorVendedorExcel(DateTime fechaInicio, DateTime fechaFinal, int slpCode = -1)
        {
            try
            {
                var archivo = await _service.FrecuenciaPorVendedorUltimaFactura(fechaInicio, fechaFinal, slpCode);

                return File(
                    archivo,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"FrecuenciaVentas_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI<string>
                {
                    EsCorrecto = false,
                    Mensaje = "Error generando el reporte.",
                    Errores = new List<string>
            {
                ex.Message
            }
                });
            }
        }
        [HttpGet("CarteraVencida/Excel")]
        public async Task<IActionResult> CarteraVencidaExcel(DateTime fechaFinal, string? cardCode = null, int? codeVendedor = null, int? codCanal = null)
        {
            try
            {
                var archivo = await _service.CarteraVencida(fechaFinal, cardCode, codeVendedor, codCanal);

                return File(
                    archivo,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"CarteraVencida_{fechaFinal:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI<string>
                {
                    EsCorrecto = false,
                    Mensaje = "Error generando el reporte.",
                    Errores = new List<string>
            {
                ex.Message
            }
                });
            }
        }

    }
}
