using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[controller]")]
    public class OK1_PICK_TPLACASController(SapDbContex sapDbContex) : Controller
    {
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var placas = await sapDbContex.OK1_PICK_TPLACAS.ToListAsync();

            if (placas == null || placas.Count == 0)
            {
                return NotFound();
            }
            return Ok(placas);

        }

    }
}
