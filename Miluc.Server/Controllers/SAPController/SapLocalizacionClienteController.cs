using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Sap.LocalizacionSap;
using Miluc.Shared.DTOs.Sap.ActividadEconomica;
using Miluc.Shared.DTOs.Sap.Cliente.HBT_RESPFISCAL;
using Miluc.Shared.DTOs.Sap.Cliente.RegimeNTributario;
using Miluc.Shared.DTOs.Sap.RegimenFiscal;
using Miluc.Shared.DTOs.Sap.Retenciones;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.SAPController
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SapLocalizacionClienteController(ILocalizacionSapCliente _sapService, ILogService log) : Controller
    {

        [HttpGet("regimen-tributario")]
        [ProducesResponseType(typeof(ResponseAPI<List<RegimenTributarioDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<RegimenTributarioDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<List<RegimenTributarioDto>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseAPI<List<RegimenTributarioDto>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseAPI<List<RegimenTributarioDto>>>> GetAllRegimenTributario()
        {
            try
            {
                var regimenTributario = await _sapService.GetAllRegimenTributarioAsync();

                return Ok(new ResponseAPI<List<RegimenTributarioDto>>
                {
                    EsCorrecto = true,
                    Valor = regimenTributario,
                    Mensaje = "Regimen Tributario obtenido correctamente",
                });
            }
            catch (ArgumentOutOfRangeException ex)
            // catch (Exception ex)
            {
                return BadRequest(new ResponseAPI<List<RegimenTributarioDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<RegimenTributarioDto>(), // Lo ideal es mantenerla inicializada
                    Mensaje = $"No se encontraron regímenes tributarios disponibles en SAP. {ex}"
                });

            }
            catch (Exception ex)
            {

                await log.GuardarErrorAsync(
                      message: ex.Message,
                       StackTrace: ex.StackTrace,
                       usuario: User.Identity?.Name ?? "Sistema",
                       metodo: nameof(GetAllRegimenTributario),
                       ruta: $"/api/regimen-tributario​",
                       ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                       origen: $"SapLocalizacionClienteController");

                return StatusCode(500, new ResponseAPI<RegimenTributarioDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        [HttpGet("tipos-documento")]
        public async Task<ActionResult<ResponseAPI<List<TiposDocumentoDto>>>> GetAllTiposDocumento()
        {
            try
            {
                var tiposDocumento = await _sapService.GetAllTiposDocumentoAsync();

                if (tiposDocumento == null)
                {
                    return NotFound();
                }
                return Ok(new ResponseAPI<List<TiposDocumentoDto>>
                {
                    EsCorrecto = true,
                    Valor = tiposDocumento,
                    Mensaje = "Tipos de Documento obtenidos correctamente",
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<TiposDocumentoDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<TiposDocumentoDto>(), // Lo ideal es mantenerla inicializada
                    Mensaje = $"No se encontraron tipos de documento disponibles en SAP. {ex}"
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetAllRegimenTributario),
                        ruta: $"/api/regimen-tributario​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"SapLocalizacionClienteController");


                return StatusCode(500, new ResponseAPI<RegimenTributarioDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }

        [ProducesResponseType(typeof(ResponseAPI<List<HbtMunicipiosDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtMunicipiosDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtMunicipiosDto>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtMunicipiosDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtMunicipiosDto>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtMunicipiosDto>>), StatusCodes.Status404NotFound)]
        [HttpGet("municipios")]
        public async Task<ActionResult<ResponseAPI<List<HbtMunicipiosDto>>>> GetAllMunicipios([FromQuery] string? buscar = null, [FromQuery] int? pagina = null, [FromQuery] int? cantidad = null)
        {
            try
            {
                var municipios = await _sapService.GetAllMunicipiosAsync(buscar, pagina, cantidad);


                return Ok(new ResponseAPI<List<HbtMunicipiosDto>>
                {
                    EsCorrecto = true,
                    Valor = municipios.data,
                    CantRegistros = municipios.cantidad,
                    Mensaje = "Municipios obtenidos correctamente",
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<HbtMunicipiosDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<HbtMunicipiosDto>(), // Lo ideal es mantenerla inicializada
                    Mensaje = $"No se encontraron municipios disponibles en SAP. {ex}",
                    Errores = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetAllMunicipios),
                        ruta: $"/api/municipios​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"SapLocalizacionClienteController");

                return StatusCode(500, new ResponseAPI<List<HbtMunicipiosDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<HbtMunicipiosDto>(), // Lo ideal es mantenerla inicializada
                    Mensaje = $"Ocurrió un error al obtener los municipios. {ex}",
                    Errores = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("responsabilidades-fiscales")]
        public async Task<ActionResult<ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>>> GetAllResponsabilidadesFiscales([FromQuery] string? buscar = null, [FromQuery] int? pagina = null, [FromQuery] int? cantidad = null)
        {
            try
            {
                var responsabilidadesFiscales = await _sapService.GetAllResponsabilidadesFiscalesAsync(buscar, pagina, cantidad);
                if (responsabilidadesFiscales == null)
                {
                    return NotFound();
                }
                return Ok(new ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>
                {
                    EsCorrecto = true,
                    Valor = responsabilidadesFiscales,
                    Mensaje = "Responsabilidades Fiscales obtenidas correctamente",
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<OkiResponsabilidadesFiscalesDto>(), // Lo ideal es mantenerla inicializada
                    Mensaje = $"No se encontraron responsabilidades fiscales disponibles en SAP. {ex}"
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetAllResponsabilidadesFiscales),
                        ruta: $"/api/responsabilidades-fiscales​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"SapLocalizacionClienteController");

                return StatusCode(500, new ResponseAPI<List<OkiResponsabilidadesFiscalesDto>>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("codigos-postales")]
        [ProducesResponseType(typeof(ResponseAPI<List<HBT_codigosPostalesDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<HBT_codigosPostalesDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<List<HBT_codigosPostalesDto>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseAPI<List<HBT_codigosPostalesDto>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseAPI<List<HBT_codigosPostalesDto>>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseAPI<List<HBT_codigosPostalesDto>>>> GetAllCodigosPostales([FromQuery] string? buscar = null, [FromQuery] int? pagina = null, [FromQuery] int? cantidad = null)
        {
            try
            {
                var resultado = await _sapService.GetAllCodigosPostalesAsync(buscar, pagina, cantidad);

                return Ok(new ResponseAPI<List<HBT_codigosPostalesDto>>
                {
                    EsCorrecto = true,
                    Valor = resultado.data,
                    CantRegistros = resultado.cantidadRegistros,
                    Mensaje = resultado.data.Any()
                        ? "Códigos postales obtenidos correctamente." : "No se encontraron códigos postales."
                });

            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<HBT_codigosPostalesDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<HBT_codigosPostalesDto>(),
                    CantRegistros = 0,
                    Mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(GetAllCodigosPostales),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: nameof(SapLocalizacionClienteController));

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseAPI<List<HBT_codigosPostalesDto>>
                    {
                        EsCorrecto = false,
                        Valor = new List<HBT_codigosPostalesDto>(),
                        CantRegistros = 0,
                        Mensaje = "Ocurrió un error interno al consultar los códigos postales.",
                        Errores = new List<string> { ex.Message }
                    });
            }
        }
        [ProducesResponseType(typeof(ResponseAPI<List<HBT_RegimenFiscalDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<HBT_RegimenFiscalDto>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseAPI<HBT_RegimenFiscalDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseAPI<HBT_RegimenFiscalDto>), StatusCodes.Status400BadRequest)]
        [HttpGet("regimen-fiscal")]
        public async Task<ActionResult<HBT_RegimenFiscalDto>> GetAllRegimenFiscal()
        {
            try
            {
                var codigosPostales = await _sapService.GetAllRegimenFiscal();


                return Ok(new ResponseAPI<List<HBT_RegimenFiscalDto>>
                {
                    EsCorrecto = true,
                    Valor = codigosPostales,
                    Mensaje = codigosPostales.Any() ? "Regímenes Fiscales obtenidos correctamente" : "No se encontraron regímenes fiscales disponibles",
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<HBT_RegimenFiscalDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<HBT_RegimenFiscalDto>(), // Lo ideal es mantenerla inicializada
                    Mensaje = $"No se encontraron regímenes fiscales disponibles en SAP. {ex}"
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                      message: ex.Message,
                       StackTrace: ex.StackTrace,
                       usuario: User.Identity?.Name ?? "Sistema",
                       metodo: nameof(GetAllRegimenFiscal),
                       ruta: $"/api/GetAllRegimenFiscal​",
                       ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                       origen: $"SapLocalizacionClienteController");

                return StatusCode(500, new ResponseAPI<HBT_RegimenFiscalDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("retenciones")]
        public async Task<ActionResult<ResponseAPI<List<RetencionDto>>>> GetAllRetncionesCliente()
        {
            try
            {
                var responsabilidadesFiscales = await _sapService.getAllRetenciones();
                return Ok(new ResponseAPI<List<RetencionDto>>
                {
                    EsCorrecto = true,
                    Valor = responsabilidadesFiscales,
                    Mensaje = responsabilidadesFiscales.Any() ? "Responsabilidades Fiscales obtenidas correctamente" : "No se encontraron responsabilidades fiscales disponibles",
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<RetencionDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<RetencionDto>(), // Lo ideal es mantenerla inicializada
                    Mensaje = $"No se encontraron responsabilidades fiscales disponibles en SAP. {ex}"
                });
            }

            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetAllRegimenFiscal),
                        ruta: $"/api/GetAllRegimenFiscal​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"SapLocalizacionClienteController");

                return StatusCode(500, new ResponseAPI<RetencionDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }

        [ProducesResponseType(typeof(ResponseAPI<List<HbtActividadEconomicaDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtActividadEconomicaDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtActividadEconomicaDto>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseAPI<List<HbtActividadEconomicaDto>>), StatusCodes.Status401Unauthorized)]
        [HttpGet("actividad-economica")]
        public async Task<ActionResult<ResponseAPI<List<HbtActividadEconomicaDto>>>> GetAllActividadEconomica([FromQuery] string? buscar = null, [FromQuery] int? pagina = null, [FromQuery] int? cantidad = null)
        {
            try
            {
                var actividadEconomica = await _sapService.GetActividadEconomica(buscar, pagina, cantidad);

                return Ok(new ResponseAPI<List<HbtActividadEconomicaDto>>
                {
                    EsCorrecto = true,
                    Valor = actividadEconomica.data,
                    Mensaje = "Responsabilidades Fiscales obtenidas correctamente",
                    CantRegistros = actividadEconomica.cantidad
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<HbtActividadEconomicaDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<HbtActividadEconomicaDto>(),
                    Mensaje = ex.Message,
                });
            }
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetAllActividadEconomica),
                        ruta: $"/api/actividad-economica​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"SapLocalizacionClienteController");
                return StatusCode(500, new ResponseAPI<RetencionDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
        [ProducesResponseType(typeof(ResponseAPI<List<SapResponsabilidadFiscalDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapResponsabilidadFiscalDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapResponsabilidadFiscalDto>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseAPI<List<SapResponsabilidadFiscalDto>>), StatusCodes.Status401Unauthorized)]
        [HttpGet("responsabilidad-fiscal1")]
        public async Task<ActionResult<ResponseAPI<List<SapResponsabilidadFiscalDto>>>> GetResponsabilidadFiscal1([FromQuery] string? buscar = null, [FromQuery] int? pagina = null, [FromQuery] int? cantidad = null)
        {
            try
            {
                var actividadEconomica = await _sapService.GetResponsabilidadFiscal1(buscar, pagina, cantidad);

                return Ok(new ResponseAPI<List<SapResponsabilidadFiscalDto>>
                {
                    EsCorrecto = true,
                    Valor = actividadEconomica,
                    Mensaje = "Responsabilidades Fiscales1  obtenidas correctamente",
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseAPI<List<SapResponsabilidadFiscalDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<SapResponsabilidadFiscalDto>(),
                    Mensaje = ex.Message,
                });
            }
        
            catch (Exception ex)
            {
                await log.GuardarErrorAsync(
                       message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetAllActividadEconomica),
                        ruta: $"/api/respnsabilidad-fiscal1​",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: $"SapLocalizacionClienteController");


                return StatusCode(500, new ResponseAPI<SapResponsabilidadFiscalDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message,
                    Errores = new List<string> { ex.Message }
                });
            }
        }
    }
}