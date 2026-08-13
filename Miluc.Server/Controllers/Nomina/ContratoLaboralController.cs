using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratoLaboralController(
        ITipoContratoService tipoContratoService,
        IEmpresaService empresaService,
        IContratoLaboralService contratoLaboralService,
        IContratoLaboralDetalleService contratoLaboralDetalleService,
        ILogService _log) : ControllerBase
    {
        //TipoContrato
        [HttpGet("TipoContrato")]

        public async Task<ActionResult<ResponseAPI<List<TipoContratoReaderDto>>>> GetAllTipoContratoAsync()
        {
            try
            {
                var tipoContratos = await tipoContratoService.GetAllTipoContratoAsync();
                if (tipoContratos == null || tipoContratos.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<TipoContratoReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron tipos de contrato.",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<TipoContratoReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = tipoContratos,
                    Mensaje = "Tipos de contrato obtenidos correctamente.",
                    CantRegistros = tipoContratos.Count
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

                return StatusCode(500, new ResponseAPI<List<TipoContratoReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los tipos de contrato.",
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
                var empresas = await empresaService.GetAllEmpresasAsync();
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
        [HttpGet("ContratoLaboral")]
        public async Task<ActionResult<ResponseAPI<List<ContratoLabralreaderDto>>>> GetContratoLaboralAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (contratoLaboral, totalRegistros) = await contratoLaboralService.GetContratoLaboralAsync(filtro, page, cantidad);

                // Si la lista es nula o vacía, devolvemos OK con lista vacía y 0 registros (no 404)
                if (contratoLaboral == null || contratoLaboral.Count == 0)
              
                {
                    return Ok(new ResponseAPI<List<ContratoLabralreaderDto>>
                    {
                        EsCorrecto = true,
                        Valor = [], // Sintaxis moderna de C# 12 para inicializar la lista vacía
                        Mensaje = "No se encontraron contratos laborales.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<List<ContratoLabralreaderDto>>
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

                return StatusCode(500, new ResponseAPI<List<ContratoLabralreaderDto>>
                {
                    EsCorrecto = false,
                    Valor = [],
                    Mensaje = "Ocurrió un error al obtener los contratos laborales.",
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseAPI<ContratoLabralreaderDto>>> GetContratoId(int id) 
        {
            var response = new ResponseAPI<ContratoLabralreaderDto>();

            try
            {
                // Ahora 'id' tendrá el valor real que viene de la URL (ej: 1, 4, 10...)
                var contratoLaboral = await contratoLaboralService.GetContratoById(id);

                if (contratoLaboral == null)
                {
                    // Es mejor retornar NotFound (404) si el contrato no existe
                    response.EsCorrecto = false;
                    response.Mensaje = $"No se encontró el contrato laboral con ID {id}";
                    return NotFound(response);
                }

                response.EsCorrecto = true;
                response.Valor = contratoLaboral;
                response.Mensaje = "Contrato obtenido exitosamente";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI<ContratoLabralreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los contratos laborales: {ex.Message}"
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseAPI<ContratoLabralreaderDto>>> UpdateAfiliaUpsertContratoLaboralAsynccionAsync(int id, [FromBody] UpdateContratoLaboralDto updateContratoLaboralDto)
        {
            // Validar null
            if (updateContratoLaboralDto == null)
            {
                return BadRequest(new ResponseAPI<ContratoLabralreaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Debe enviar la información del contrato laboral.",
                    CantRegistros = 0
                });
            }

            // Validar que el id de la URL coincida con el DTO
            if (id != updateContratoLaboralDto.ContratoLaboralId)
            {
                return BadRequest(new ResponseAPI<ContratoLabralreaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "El id enviado no coincide.",
                    CantRegistros = 0
                });
            }

            // Validar modelo
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Campo = x.Key,
                        Errores = x.Value.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                return BadRequest(new ResponseAPI<object>
                {
                    EsCorrecto = false,
                    Valor = errores,
                    Mensaje = "Datos inválidos.",
                    CantRegistros = 0
                });
            }

            try
            {
                // Upsert devuelve bool -> no es un DTO
                var resultado = await contratoLaboralService.UpsertContratoLaboralAsync(updateContratoLaboralDto);

                if (!resultado)
                {
                    return NotFound(new ResponseAPI<ContratoLabralreaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontró o no se pudo actualizar el contrato laboral.",
                        CantRegistros = 0
                    });
                }

                return Ok(new ResponseAPI<ContratoLabralreaderDto>
                {
                    EsCorrecto = true,
                    Valor = null, // El servicio solo retorna boolean, para devolver DTO se necesita consultar de nuevo
                    Mensaje = "Contrato laboral actualizado correctamente.",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpPut",
                    ruta: $"/api/ContratoLaboral/UpdateContratoLaboral/{id}",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "ContratoLaboralController"
                );

                return StatusCode(500, new ResponseAPI<ContratoLabralreaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al actualizar el contrato laboral.",
                    CantRegistros = 0
                });
            }
        }
    }

}



