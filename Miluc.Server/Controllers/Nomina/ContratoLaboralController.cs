using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Miluc.Client.Interfaces.Nomina;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.Cargos;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using Miluc.Shared.DTOs.Sap.Pedidos;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratoLaboralController(
        ITipoContratoService tipoContratoService,
        ICargoService cargoService,
        IEmpresaService empresaService,
        IContratoLaboralService contratoLaboralService,
        IContratoLaboralDetalleService contratoLaboralDetalleService,
        ILogService _log) : ControllerBase
    {
        //TipoContrato

        [HttpGet("TipoContrato")]
        public async Task<ActionResult<ResponseAPI<List<TipoContratoReaderDto>>>> GetTipoContratoAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var tipoContrato = await tipoContratoService.GetTipoContratoAsync(filtro, page, cantidad);
                if (tipoContrato.Data == null || tipoContrato.Data.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<TipoContratoReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron Contratos",
                        CantRegistros = 0,
                    });
                }



                return Ok(new ResponseAPI<List<TipoContratoReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = tipoContrato.Data,
                    Mensaje = "Se obtuvieron las Contratos correctamente",
                    CantRegistros = tipoContrato.TotalRegistros,
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: "/api/TipoContrato",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "TipoContratoController");

                return StatusCode(500, new ResponseAPI<List<TipoContratoReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "",
                    CantRegistros = 0
                });
            }
        }
        //Empresa
        [HttpGet("Empresa")]
        public async Task<ActionResult<ResponseAPI<List<EmpresaReaderDto>>>> GetAllEmpresasAsync()
        {
            try
            {
                var empresas = await empresaService.GetEmpresasAsync();
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
              origen: nameof(ContratoLaboralController));

                return StatusCode(500, new ResponseAPI<List<EmpresaReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener las empresas.",
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("Cargos")]
        public async Task<ActionResult<ResponseAPI<List<CargosDto>>>> GetCargosAsyc([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (cargo, totalRegistros) = await cargoService.GetCargosAsyc(filtro, page, cantidad);

                // Si la lista es nula o vacía, devolvemos OK con lista vacía y 0 registros (no 404)
                if (cargo == null || cargo.Count == 0)
                {
                    return Ok(new ResponseAPI<List<CargosDto>> // CORREGIDO: List<CargosDto>
                    {
                        EsCorrecto = true,
                        Valor = [],
                        Mensaje = "No se encontraron cargos.", // CORREGIDO
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<CargosDto>>
                {
                    EsCorrecto = true,
                    Valor = cargo,
                    Mensaje = "Cargos obtenidos correctamente.", // CORREGIDO
                    CantRegistros = totalRegistros
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: nameof(GetCargosAsyc),
                    ruta: HttpContext.Request.Path,
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "cargos");

                return StatusCode(500, new ResponseAPI<List<CargosDto>>
                {
                    EsCorrecto = false,
                    Valor = [],
                    Mensaje = "Ocurrió un error al obtener los cargos.",
                    CantRegistros = 0
                });
            }
        }

        [HttpGet("ContratoLaboralDetalle")]
        public async Task<ActionResult<ResponseAPI<List<ContratoLaboralDetalleDto>>>> GetAllContratoLaboralDetallesAsync()
        {
            try
            {
                var contratoLaboralDetalles = await contratoLaboralDetalleService.GetContratoLaboralDetallesAsync();
                if (contratoLaboralDetalles == null || contratoLaboralDetalles.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<ContratoLaboralDetalleDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron detalles de contratos laborales.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<ContratoLaboralDetalleDto>>
                {
                    EsCorrecto = true,
                    Valor = contratoLaboralDetalles,
                    Mensaje = "Detalles de contratos laborales obtenidos correctamente.",
                    CantRegistros = contratoLaboralDetalles.Count
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
             message: ex.Message,
             StackTrace: ex.StackTrace,
             usuario: User.Identity?.Name ?? "Sistema",
             metodo: nameof(GetContratoLaboralAsync),
             ruta: HttpContext.Request.Path,
             ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
             origen: nameof(ContratoLaboralController));
                return StatusCode(500, new ResponseAPI<List<ContratoLaboralDetalleDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los detalles de contratos laborales.",
                    CantRegistros = 0
                });
            }
        }
 
    
        
            // 1. OBTENER TODOS / FILTRAR (GET: api/ContratoLaboral)
            [HttpGet]
            public async Task<ActionResult<ResponseAPI<List<ContratoLaboralreaderDto>>>> GetContratoLaboralAsync(
                [FromQuery] string? filtro = null,
                [FromQuery] int page = 1,
                [FromQuery] int? cantidad = null)
            {
                try
                {
                    var (contratoLaboral, totalRegistros) = await contratoLaboralService.GetContratoLaboralAsync(filtro, page, cantidad);

                    if (contratoLaboral == null || contratoLaboral.Count == 0)
                    {
                        return Ok(new ResponseAPI<List<ContratoLaboralreaderDto>>
                        {
                            EsCorrecto = true,
                            Valor = [],
                            Mensaje = "No se encontraron contratos laborales.",
                            CantRegistros = 0
                        });
                    }

                    return Ok(new ResponseAPI<List<ContratoLaboralreaderDto>>
                    {
                        EsCorrecto = true,
                        Valor = contratoLaboral,
                        Mensaje = "Contratos laborales obtenidos correctamente.",
                        CantRegistros = totalRegistros
                    });
                }
                catch (Exception ex)
                {
                    await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetContratoLaboralAsync),
                        ruta: HttpContext.Request.Path,
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(ContratoLaboralController));

                    return StatusCode(500, new ResponseAPI<List<ContratoLaboralreaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = [],
                        Mensaje = "Ocurrió un error al obtener los contratos laborales.",
                        CantRegistros = 0
                    });
                }
            }

            // 2. CREAR (POST: api/ContratoLaboral)
            [HttpPost]
            public async Task<ActionResult<ResponseAPI<bool>>> CreateContratoLaboralAsync([FromBody] CreateContratoLaboralDto createContratos)
            {
                if (createContratos == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = "Información inválida o campos requeridos incompletos.",
                        CantRegistros = 0
                    });
                }

                try
                {
                    var resultado = await contratoLaboralService.CreateContratoLaboralAsync(createContratos);

                    if (!resultado)
                    {
                        return BadRequest(new ResponseAPI<bool>
                        {
                            EsCorrecto = false,
                            Valor = false,
                            Mensaje = "No fue posible registrar la información del contrato.",
                            CantRegistros = 0
                        });
                    }

                    return Ok(new ResponseAPI<bool>
                    {
                        EsCorrecto = true,
                        Valor = true,
                        Mensaje = "Información de contrato laboral registrada correctamente.",
                        CantRegistros = 1
                    });
                }
                catch (Exception ex)
                {
                    await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(CreateContratoLaboralAsync),
                        ruta: HttpContext.Request.Path,
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(ContratoLaboralController));

                    return StatusCode(500, new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje = $"Error interno al registrar el contrato: {ex.Message}",
                        CantRegistros = 0
                    });
                }
            }

            // 3. OBTENER POR EMPLEADO (GET: api/ContratoLaboral/empleado/5)
            [HttpGet("empleado/{id}")]
            public async Task<ActionResult<ResponseAPI<List<ContratoLaboralreaderDto>>>> GetContratosPorEmpleadoAsync(int id)
            {
                try
                {
                    var contratos = await contratoLaboralService.GetContratosPorEmpleadoAsync(id);

                    if (contratos == null || !contratos.Any())
                    {
                        return NotFound(new ResponseAPI<List<ContratoLaboralreaderDto>>
                        {
                            EsCorrecto = false,
                            Valor = null,
                            Mensaje = $"No se encontraron contratos para el empleado con ID {id}.",
                            CantRegistros = 0
                        });
                    }

                    return Ok(new ResponseAPI<List<ContratoLaboralreaderDto>>
                    {
                        EsCorrecto = true,
                        Valor = contratos,
                        Mensaje = "Información de contratos laborales obtenida correctamente.",
                        CantRegistros = contratos.Count
                    });
                }
                catch (Exception ex)
                {
                    await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetContratosPorEmpleadoAsync),
                        ruta: HttpContext.Request.Path,
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(ContratoLaboralController));

                    return StatusCode(500, new ResponseAPI<List<ContratoLaboralreaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"Error al obtener contratos del empleado: {ex.Message}",
                        CantRegistros = 0
                    });
                }
            }

            // 4. OBTENER POR ID (GET: api/ContratoLaboral/5)
            [HttpGet("{id}")]
            public async Task<ActionResult<ResponseAPI<ContratoLaboralreaderDto>>> GetContratoByIdAsync(int id)
            {
                try
                {
                    var contrato = await contratoLaboralService.GetContratoByIdAsync(id);

                    if (contrato == null)
                    {
                        return NotFound(new ResponseAPI<ContratoLaboralreaderDto>
                        {
                            EsCorrecto = false,
                            Valor = null,
                            Mensaje = $"No se encontró el contrato laboral con el ID {id}.",
                            CantRegistros = 0
                        });
                    }

                    return Ok(new ResponseAPI<ContratoLaboralreaderDto>
                    {
                        EsCorrecto = true,
                        Valor = contrato,
                        Mensaje = "Información del contrato laboral obtenida correctamente.",
                        CantRegistros = 1
                    });
                }
                catch (Exception ex)
                {
                    await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(GetContratoByIdAsync),
                        ruta: HttpContext.Request.Path,
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(ContratoLaboralController));

                    return StatusCode(500, new ResponseAPI<ContratoLaboralreaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"Error al obtener el contrato: {ex.Message}",
                        CantRegistros = 0
                    });
                }
            }

            // 5. ACTUALIZAR POR ID (PUT: api/ContratoLaboral/5)
            [HttpPut("{id}")]
            public async Task<ActionResult<ResponseAPI<ContratoLaboralreaderDto>>> UpdateContratoAsync(int id, [FromBody] UpdateContratoDto updateContratoDto)
            {
                try
                {
                    if (updateContratoDto == null)
                    {
                        return BadRequest(new ResponseAPI<ContratoLaboralreaderDto>
                        {
                            EsCorrecto = false,
                            Valor = null,
                            Mensaje = "Los datos para actualizar están vacíos.",
                            CantRegistros = 0
                        });
                    }

                    var resultado = await contratoLaboralService.UpdateContratoAsync(updateContratoDto);

                    if (resultado == null)
                    {
                        return NotFound(new ResponseAPI<ContratoLaboralreaderDto>
                        {
                            EsCorrecto = false,
                            Valor = null,
                            Mensaje = "No se pudo encontrar o actualizar el contrato laboral.",
                            CantRegistros = 0
                        });
                    }

                    return Ok(new ResponseAPI<ContratoLaboralreaderDto>
                    {
                        EsCorrecto = true,
                        Valor = resultado,
                        Mensaje = "Estado del contrato actualizado correctamente.",
                        CantRegistros = 1
                    });
                }
                catch (Exception ex)
                {
                    await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(UpdateContratoAsync),
                        ruta: HttpContext.Request.Path,
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(ContratoLaboralController));

                    return StatusCode(500, new ResponseAPI<ContratoLaboralreaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = $"Error al actualizar el contrato: {ex.Message}",
                        CantRegistros = 0
                    });
                }
            }

            // 6. INHABILITAR (PUT: api/ContratoLaboral/Inhabilitar)
            [HttpPut("Inhabilitar")]
            public async Task<ActionResult<ResponseAPI<bool>>> InhabilitarContratoAsync([FromBody] InhabilitarContratoDto inhabilitarDto)
            {
                try
                {
                    var resultado = await contratoLaboralService.InhabilitarContratoAsync(inhabilitarDto);

                    if (!resultado)
                    {
                        return NotFound(new ResponseAPI<bool>
                        {
                            EsCorrecto = false,
                            Valor = false,
                            Mensaje = "No se pudo encontrar el contrato laboral o su detalle asociado.",
                            CantRegistros = 0
                        });
                    }

                    return Ok(new ResponseAPI<bool>
                    {
                        EsCorrecto = true,
                        Valor = true,
                        Mensaje = "Contrato inhabilitado correctamente.",
                        CantRegistros = 1
                    });
                }
                catch (Exception ex)
                {
                    await _log.GuardarErrorAsync(
                        message: ex.Message,
                        StackTrace: ex.StackTrace,
                        usuario: User.Identity?.Name ?? "Sistema",
                        metodo: nameof(InhabilitarContratoAsync),
                        ruta: HttpContext.Request.Path,
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: nameof(ContratoLaboralController));

                    return StatusCode(500, new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Valor = false,
                        Mensaje =ex.Message,
                        CantRegistros = 0
                    });
                }
            }
        }
    }









