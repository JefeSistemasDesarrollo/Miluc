using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Encriptacion;
using Miluc.Shared.DTOs.Sap.ConexionSapServiceLayer;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Generales
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class CambiarController(MilucDbContext _contex, IEncryptionService _encryptionService) : Controller
    {
        [HttpPut("ActualizarConfiguraciones")]
        public async Task<ActionResult<ResponseAPI<bool>>> ActualizarConfiguraciones([FromBody] ConexionSapServiceLayerDto config)
        {

            try
            {
                var configuracion = await _contex.SisConfiguracionesGenerales
                .FirstOrDefaultAsync(x => x.Modulo == "xxxSAPxxModificarxxx");

                if (configuracion == null)
                {
                    return BadRequest(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se realizo el cambio"
                    });
                }
                else
                {
                    configuracion.UserNameServiceLayer = config.UserName;
                    configuracion.PasswordServiceLayer = _encryptionService.Encrypt(config.Password);

                    await _contex.SaveChangesAsync();
                }


                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Mensaje = " Actualizada Correctamente",
                    Valor = true
                });

            }
            catch (Exception ex)
            {
             return StatusCode(500, new ResponseAPI<bool>
              {
                  EsCorrecto = false,
                  Mensaje=$"error {ex.Message}"
              });
            }
        }
    }
}
