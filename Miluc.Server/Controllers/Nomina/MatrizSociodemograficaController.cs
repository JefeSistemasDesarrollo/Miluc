using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatrizSociodemograficaController(IMatrizSociodemograficaService matrizSociodemograficaService, ILogService _log) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ResponseAPI<List<MatrizSociodemograficaReaderDto>>>> GetMatrizSociodemograficaAsync([FromQuery] string? filtro = null, [FromQuery] int page = 1, [FromQuery] int? cantidad = null)
        {
            try
            {
                var (matrizSociodemografica, totalRegistros) = await matrizSociodemograficaService.GetMatrizSocioDemograficasAsync(filtro, page, cantidad);
                if (matrizSociodemografica == null || matrizSociodemografica.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<MatrizSociodemograficaReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron registros de la matriz sociodemográfica",
                        CantRegistros = 0,
                    });
                }
                return Ok(new ResponseAPI<List<MatrizSociodemograficaReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = matrizSociodemografica,
                    Mensaje = "Se obtuvieron los registros de la matriz sociodemográfica correctamente",
                    CantRegistros = totalRegistros,
                });
            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                    message: ex.Message,
                    StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                    ruta: "/api/MatrizSociodemografica",
                    ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "MatrizSociodemograficaController");
                return StatusCode(500, new
                {
                    EsCorrecto = false,
                    Valor = (object?)null,
                    Mensaje = "Ocurrió un error al obtener los registros de la matriz sociodemográfica.",
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<MatrizSociodemograficaReaderDto>>> GetMatrizSocioDemograficaByIdAsync(int Id)
        {
            try
            {
                var matriz = await matrizSociodemograficaService.GetMatrizPorEmpleadoIdAsync(Id);
                if (matriz == null)
                {
                    return NotFound(new ResponseAPI<AfiliacionSeguridadSocialreaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se encontro matriz.",
                        Valor = null,
                        CantRegistros = 0,
                    });
                }
                return Ok(new ResponseAPI<MatrizSociodemograficaReaderDto>
                {
                    EsCorrecto = true,
                    Valor = matriz,
                    Mensaje = "se encontró la MatrizSociodemografica correctamente ",
                    CantRegistros = 1
                });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                   message: ex.Message,
                   StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                   ruta: $"/api/controller",
                   ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "MatrizSociodemograficaController");

                return StatusCode(500, new ResponseAPI<MatrizSociodemograficaReaderDto>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener la MatrizSociodemografica.",
                    CantRegistros = 0
                });
            }
        }
            [HttpPut("{id}")]
            public async Task<ActionResult<ResponseAPI<MatrizSociodemograficaReaderDto>>> updateMatrizAsync(int id, [FromBody]MatrizSocioDemograficaUpdateDto matrizUpdateDto)
            {
                // Validar null
                if (matrizUpdateDto == null)
                {
                    return BadRequest(new ResponseAPI<MatrizSociodemograficaReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "Debe enviar la información de la matriz sociodemográfica.",
                        CantRegistros = 0
                    });
                }

                // Validar que el id de la URL coincida con el DTO
                if (id != matrizUpdateDto.MatrizSociodemograficaId)
                {
                    return BadRequest(new ResponseAPI<MatrizSociodemograficaReaderDto>
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
                    var resultado = await matrizSociodemograficaService.updateMatrizAsync(matrizUpdateDto);

                    if (!resultado)
                    {
                        return NotFound(new ResponseAPI<MatrizSociodemograficaReaderDto>
                        {
                            EsCorrecto = false,
                            Valor = null,
                            Mensaje = "No se encontró o no se pudo actualizar la matriz sociodemográfica.",
                            CantRegistros = 0
                        });
                    }

                    return Ok(new ResponseAPI<MatrizSociodemograficaReaderDto>
                    {
                        EsCorrecto = true,
                        Valor = null, // El servicio solo retorna boolean, para devolver DTO se necesita consultar de nuevo
                        Mensaje = "Matriz sociodemográfica actualizada correctamente.",
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
                        ruta: $"/api/MatrizSociodemografica/{id}",
                        ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                        origen: "MatrizSociodemograficaController"
                    );

                    return StatusCode(500, new ResponseAPI<MatrizSociodemograficaReaderDto>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "Ocurrió un error al actualizar la matriz sociodemográfica.",
                        CantRegistros = 0
                    });
                }
            }
        }
        }

    
