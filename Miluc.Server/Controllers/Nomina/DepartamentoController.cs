using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.DTOs.Nomina.MunicipioDto;
using Miluc.Shared.DTOs.Nomina.NewFolder;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController(IDepartamentoService departamentoService, IMunicipioService municipioService, ILogService _log) : Controller
    {
        [HttpGet("Departamentos")]
        public async Task<ActionResult<ResponseAPI<List<DepartamentoReaderDto>>>> GetDepartamentosAsync()
        {
            try
            {
                var departamentos = await departamentoService.GetDepartamentosAsync();
                if (departamentos == null || departamentos.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<DepartamentoReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron departamentos.",
                        Valor = null,
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<DepartamentoReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Departamentos obtenidos exitosamente.",
                    Valor = departamentos,
                    CantRegistros = departamentos.Count
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: "HttpGet",
                        ruta: $"/api/TipoContrato",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: "TipoContratoController");



            }
            return StatusCode(500, new ResponseAPI<List<ContratoLabralreaderDto>>
            {
                EsCorrecto = false,
                Valor = null,
                Mensaje = "Ocurrió un error al obtener Departamentos.",
                CantRegistros = 0
            });

        }
        [HttpGet ("Municipio")]
        public async Task<ActionResult<ResponseAPI<List<MunicipioReaderDto>>>> GetMunicipiosAsync()
        {
            try
            {
                var municipios = await municipioService.GetMunicipiosAsync();

                if (municipios == null || municipios.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<MunicipioReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontraron municipios.",
                        Valor = null,
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<MunicipioReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Municipios obtenidos exitosamente.",
                    Valor = municipios,
                    CantRegistros = municipios.Count
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "GET",
                    ruta: "/api/Departamento/Municipios",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "DepartamentoController"
                );

                return StatusCode(500, new ResponseAPI<List<MunicipioReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener Municipios.",
                    CantRegistros = 0
                });
            }
        }
    }
}
