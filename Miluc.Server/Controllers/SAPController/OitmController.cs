using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.Oitm;
using Miluc.Shared.DTOs.Sap.Articulos;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    
    [ApiController]
    [Route("api/[Controller]")]
    public class OitmController(ISapOitmService oitmService) : Controller
    {

        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<ArticuloPorListaDePreciosDto>>>> GetListOitm([FromQuery] string? buscar = null, [FromQuery] int pagina = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var listOitm = await oitmService.GetListOitmAsync(buscar, pagina, cantidad);
                // Verificar si la lista está vacía o es nula
                if (listOitm.Data == null || listOitm.Data.Count == 0)
                {
                    // Retornar una respuesta indicando que no se encontraron OITM
                    return NotFound(new ResponseAPI<List<ArticuloPorListaDePreciosDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron OITM",
                        Valor = null
                    });
                }

                // Retornar la lista de OITM en una respuesta exitosa
                return Ok(new ResponseAPI<List<SapOitmDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de OITM",
                    Valor = listOitm.Data,
                    CantRegistros = listOitm.TotalRegistros
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI<List<ArticuloPorListaDePreciosDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener la lista de OITM: {ex.Message}",
                    Valor = null
                });
            }
        }

        // GET: api/Oitm/oitmCardcodeItemCode?CardCode=123&ItemCode=ABC&ItemName=Example&Quantity=10

        [HttpGet("oitmCardcodeItemCode")]
        public async Task<ActionResult<ResponseAPI<ArticuloPorListaDePreciosDto>>>
       GetOitmAsync([FromQuery] string cardcode, [FromQuery] string itemcode, [FromQuery] int? cantidad = null)
        {
            try
            {
                var listOitm = await oitmService.GetListOitmCarcodeItemcodeAsync(cardcode, itemcode, cantidad);


                return Ok(new ResponseAPI<ArticuloPorListaDePreciosDto>
                {
                    EsCorrecto = true,
                    Mensaje = "Lista de OITM",
                    Valor = listOitm
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI<ArticuloPorListaDePreciosDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener la lista de OITM: {ex.Message}",
                    Valor = null
                });
            }

        }
    }
}
