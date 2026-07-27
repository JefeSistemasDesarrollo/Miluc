using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.Sap.Opln;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Opln;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OplnController (ISapOplnService sapOplnService): Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<OplnReaderDto>>>> GetOplnAsync()
        {
            try
            {
                var listOpln = await sapOplnService.GetListOplnAsync();

                return Ok(new ResponseAPI<List<OplnReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Consulta Exitosa",
                    Valor = listOpln,
                });

            }
            catch (Exception ex)
            {
                throw new Exception("" + ex.Message);
            }
        }
    }
}
