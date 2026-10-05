using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.Models.Response;
using System.Text;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresaController(IEmpresaService _empresaService, ILogService _log) : Controller
    {
        //Empresa
        [HttpGet("Empresa")]
        public async Task<ActionResult<ResponseAPI<List<EmpresaReaderDto>>>> GetAllEmpresasAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {

                var empresas = await _empresaService.GetEmpresasAsync(filtro, page, cantidad);
                if (empresas == null || empresas.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<EmpresaReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron empresas.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<EmpresaReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = empresas,
                    Mensaje = "Empresas obtenidas correctamente.",
                    CantRegistros = empresas.Count
                });
            }
            catch (Exception ex)
            {


                await _log.GuardarErrorAsync(
              message: ex.Message,
              StackTrace: ex.StackTrace,
              usuario: User.Identity?.Name ?? "Sistema",
              metodo: nameof(GetAllEmpresasAsync),
              ruta: HttpContext.Request.Path,
              ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
              origen: nameof(EmpresaController));

                return StatusCode(500, new ResponseAPI<List<EmpresaReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener las empresas.",
                    CantRegistros = 0
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> UpsertEmpresaAsync(int id, [FromBody] UpsertEmpresaDto empresaDto)
        {
            if (empresaDto == null)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Debe enviar la información de la empresa.",
                    CantRegistros = 0
                });
            }

            // Si viene ID en la ruta pero el DTO trae 0, se sincronizan; si ambos traen IDs distintos, se rechaza.
            if (empresaDto.EmpresaId != 0 && id != empresaDto.EmpresaId)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "El Id de la URL no coincide con el Id enviado.",
                    CantRegistros = 0
                });
            }

            if (empresaDto.EmpresaId == 0 && id > 0)
            {
                empresaDto.EmpresaId = id;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Datos inválidos.",
                    CantRegistros = 0
                });
            }

            try
            {
                bool esNueva = empresaDto.EmpresaId == 0;
                var resultado = await _empresaService.UpsertEmpresaAsync(empresaDto);

                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = resultado,
                    Mensaje = esNueva
                        ? "Empresa creada correctamente."
                        : "Empresa actualizada correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                var errorReal = ObtenerMensajeDetallado(ex);

                await _log.GuardarErrorAsync(
                    message: errorReal,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(UpsertEmpresaAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(EmpresaController)
                );

                // Seguridad: Se registra el error detallado internamente, pero se responde un mensaje controlado al cliente.
                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Ocurrió un error interno al procesar la solicitud.",
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<EmpresaReaderDto>>> GetEmpresaByIdAsync(int id)
        {
            try
            {
                var empresa = await _empresaService.GetEmpresaByIdAsync(id);
                if (empresa == null)
                {
                    return NotFound(new ResponseAPI<EmpresaReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"No se encontró la empresa con ID {id}.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<EmpresaReaderDto>
                {
                    EsCorrecto = true,
                    Valor = empresa,
                    Mensaje = "Empresa obtenida correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(GetEmpresaByIdAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(EmpresaController)
                );
                return StatusCode(500, new ResponseAPI<EmpresaReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener la empresa.",
                    CantRegistros = 0
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseAPI<bool>>> DeleteEmpresaAsync(int id)
        {
            try
            {
                var resultado = await _empresaService.DeleteEmpresaAsync(id);
                if (!resultado)
                {
                    return NotFound(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = $"No se encontró la empresa con ID {id}.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<bool>
                {
                    EsCorrecto = true,
                    Valor = true,
                    Mensaje = "Empresa eliminada correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(DeleteEmpresaAsync),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(EmpresaController)
                );
                return StatusCode(500, new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Valor = false,
                    Mensaje = "Ocurrió un error al eliminar la empresa.",
                    CantRegistros = 0
                });
            }
        }

        private static string ObtenerMensajeDetallado(Exception ex)
        {
            var sb = new StringBuilder(ex.Message);
            var actual = ex.InnerException;

            while (actual != null)
            {
                sb.Append(" | InnerException: ").Append(actual.Message);
                actual = actual.InnerException;
            }

            return sb.ToString();
        } 
    }
}