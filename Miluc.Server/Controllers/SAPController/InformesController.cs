using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.Informes.InformeComercial;
using Miluc.Server.Security;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class InformesController(ISapInformeService _service) : Controller
    {
        [HttpGet("FacturasPorVendedor/Excel")]
        [PermissionAuthorize("Informe.FacturasVentas")]
        public async Task<IActionResult> FacturasPorVendedorExcel(DateTime fechaInicio, DateTime fechaFinal, int slpCode = -1, int tipoInforme = 0)
        {
            try
            {
                Stream stream = new MemoryStream();
                string nombreArchivo = $"InformeVentas";

                if (tipoInforme == 1)
                {
                    stream = await _service.FacturasPorVendedorUltimaFactura(fechaInicio, fechaFinal, slpCode);
                    string nombreArchivoDetalle = $"FacturasPorVendedorCabeceraFacturas_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx";

                }
                if (tipoInforme == 2)
                {
                    stream = await _service.FacturasPorVendedorUltimaFacturaDetalle(fechaInicio, fechaFinal, slpCode);
                    string nombreArchivoResumen = $"FacturasPorVendedorFacturaDetalle_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx";
                }

                if (tipoInforme == 3)
                {
                    stream = await _service.FacturasPorVendedorCabeceraHistoricoFacturas(slpCode);
                    string nombreArchivoHistorico = $"FacturasPorVendedorCabeceraHistoricoFacturas.xlsx";

                }

                if (tipoInforme == 4)
                {
                    stream = await _service.FacturasPorVendedorCabeceraHistoricoFacturasDetalle(slpCode);
                    string nombreArchivoHistoricoDetalle = $"FacturasPorVendedorCabeceraHistoricoFacturasDetalle.xlsx";
                }

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                     nombreArchivo);

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
        [PermissionAuthorize("Informe.CarteraClientes")]
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
        [HttpGet("CanalDeDistribucion/Excel")]
        [PermissionAuthorize("Informe.CanalaDeDistribucion")]
        public async Task<IActionResult> CanalDeDistribucionExcel(DateTime fechaInicio, DateTime fechaFinal, int? GrDistribucion = null)
        {
            try
            {
                var archivo = await _service.CanalDeDistribucion(fechaInicio, fechaFinal, GrDistribucion);

                return File(
                    archivo,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"CanalDeDistribucion_{fechaInicio:yyyyMMdd}_{fechaFinal:yyyyMMdd}.xlsx");
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
        [HttpGet("Trazabilidad/Excel")]
        [PermissionAuthorize("Informe.Trazabilidad")]
        public async Task<IActionResult> TrazabilidadOrden(DateTime FechaIncio, DateTime FechaFinal, int? CodeVendedor = null)
        {
            try
            {
                var archivo = await _service.TrazabilidadFacturas(FechaIncio.Date, FechaFinal.Date, CodeVendedor);
                
                return File(
                    archivo,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Trazabilidad_{FechaIncio:yyyyMMdd}_{FechaFinal:yyyyMMdd}.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI<string>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al generera trazabiliad de ventas",
                    Errores = new List<string> { ex.Message }
                });
            }
        }
    }
}
