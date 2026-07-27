using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.BussnesParnerGrup;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Credito;
using Miluc.Shared.DTOs.Sap.GrupoDeVenta;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BusinessPartnerGroupsController(IBusinessPartnerGroups _PartnerGroupsService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<BusinessPartnerGroupsDto>>> GetAllBusinessPartnerGroups()
        {
            try
            {
                var result = await _PartnerGroupsService.GetallBusinessPartnerGroups();

                if (result == null) 
                {
                    return NotFound("No se encontraron grupos de socios comerciales.");

                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los grupos de socio de negocio: {ex.Message}");
            }

        }

    }
}
