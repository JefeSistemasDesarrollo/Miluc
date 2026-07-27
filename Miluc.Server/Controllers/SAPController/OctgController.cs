using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.Octg;
using Miluc.Shared.DTOs.Sap.Credito;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{

    [ApiController]
    [Route("api/[Controller]")]

    public class OctgController (ISapOctgService _octgService): Controller
    {
       [HttpGet]
       public async Task<ActionResult<ResponseAPI<List<DiasCreditoDto>>>> GetAllDiasCreditoAsync()
        {
            try
            {
                var result = await _octgService.GetAllDiasCreditoAsync();

                if (result == null)
                {
                    return NotFound("No se encontraron grupos de socios comerciales.");

                }

                return Ok(new ResponseAPI<List<DiasCreditoDto>>
                {
                    EsCorrecto = true,
                    Mensaje="Lista de clientes consultada correctamente",
                    Valor=result

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los grupos de socio de negocio: {ex.Message}");
            }
        }
    }
}
