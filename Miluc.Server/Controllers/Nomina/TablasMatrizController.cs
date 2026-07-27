using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.ClaseVivienda;
using Miluc.Shared.DTOs.Nomina.CondicionMedica;
using Miluc.Shared.DTOs.Nomina.DeporteRederDto;
using Miluc.Shared.DTOs.Nomina.GeneroDto;
using Miluc.Shared.DTOs.Nomina.MedioTransporteDto;
using Miluc.Shared.DTOs.Nomina.NivelAcademico;
using Miluc.Shared.DTOs.Nomina.PaisDto;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using Miluc.Shared.DTOs.Nomina.TipoVivienda;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Controllers.Nomina
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablasMatrizController(IGeneroService generoService, IPaisService paisService, IDeporteService deporteService, ICondicionMedicaService condicionMedicaService, INivelAcademicoService nivelAcademicoService,
        IMedioTransporteService medioTransporteService, ITipoViviendaService tipoViviendaService, IClaseViviendaService claseViviendaService, ILogService _log) : Controller
    {
        [HttpGet("Genero")]
        public async Task<ActionResult<ResponseAPI<List<GeneroReaderDto>>>> GetGeneroAsync()
        {
            try
            {
                var gen = await generoService.GetGeneroAsync();
                if (gen == null || gen.Count == 0)
                {
                    return NotFound(new ResponseAPI<List<GeneroReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron Generos",
                        CantRegistros = 0,
                    });
                }
                return Ok(new ResponseAPI<List<GeneroReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = gen,
                    Mensaje = "Se encontraron generos correctamente",
                    CantRegistros = gen.Count,
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
                origen: "GeneroController");

                return StatusCode(500, new ResponseAPI<List<GeneroReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los Generos.",
                    CantRegistros = 0
                });

            }
        }
        [HttpGet("Pais")]
        public async Task<ActionResult<ResponseAPI<List<PaisReaderDto>>>> GetPaisAsync([FromQuery] string ? filtro=null, [FromQuery]  int page  = 1, [FromQuery] int ?cantidad = null)

        {

          
            try
            {
                var pais = await paisService.GetPaisAsync(filtro, page, cantidad);
               
                if (pais.data == null || pais.CantidadRegistros == 0)
                {
                    return NotFound(new ResponseAPI<List<PaisReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No se encontraron paises",
                        CantRegistros = 0
                    });
                }
                return Ok(new ResponseAPI<List<PaisReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = pais.data,
                    Mensaje = "Se encontraron paises correctamente",
                    CantRegistros = pais.CantidadRegistros


                });

            }

            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
                message: ex.Message,
                StackTrace: ex.StackTrace,
                 usuario: User.Identity?.Name ?? "Sistema",
                 metodo: "HttpGet",
                ruta: $"/api/Pais",
                ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                 origen: "GeneroController");

                return StatusCode(500, new ResponseAPI<List<PaisReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los pais.",
                    CantRegistros = 0
                });


            }
        }
        [HttpGet("Deportes")]
        public async Task<ActionResult<ResponseAPI<List<DeporteRederDto>>>> GetDeporteAsync()
        {
            try
            {
                var deporte = await deporteService.GetDeporteAsync();
                if (deporte == null || deporte.Count == 0)
                {
                    //si no retorna Deportes
                    return NotFound(new ResponseAPI<List<DeporteRederDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "no se encontraron Deportes",
                        CantRegistros = 0,

                    });
                }
                return Ok(new ResponseAPI<List<DeporteRederDto>>
                {

                    EsCorrecto = true,
                    Valor = deporte,
                    Mensaje = "Se encontraron Deportes correctamente",
                    CantRegistros = deporte.Count

                });

            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
             message: ex.Message,
             StackTrace: ex.StackTrace,
              usuario: User.Identity?.Name ?? "Sistema",
              metodo: "HttpGet",
             ruta: $"/api/Deportes",
             ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
              origen: "DeportesController");

                return StatusCode(500, new ResponseAPI<List<DeporteRederDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los Deporte.",
                    CantRegistros = 0
                });




            }
        }
        [HttpGet("CondicionMedica")]
        public async Task<ActionResult<ResponseAPI<List<CondicionMedicaReaderDto>>>> GetCondicionMedicaAsync()
        {
            try
            {
                var condicion = await condicionMedicaService.GetCondicionMedicaAsync();
                if (condicion == null || condicion.Count == 0)
                {
                    //si no retorna Deportes
                    return NotFound(new ResponseAPI<List<CondicionMedicaReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "no se encontraron condiciones medicas",
                        CantRegistros = 0,

                    });
                }
                return Ok(new ResponseAPI<List<CondicionMedicaReaderDto>>
                {

                    EsCorrecto = true,
                    Valor = condicion,
                    Mensaje = "Se encontraron condiciones medicas correctamente",
                    CantRegistros = condicion.Count

                });

            }
            catch (Exception ex)
            {
                await _log.GuardarErrorAsync(
             message: ex.Message,
             StackTrace: ex.StackTrace,
              usuario: User.Identity?.Name ?? "Sistema",
              metodo: "HttpGet",
             ruta: $"/api/CondicionMedica",
             ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
              origen: "CondicionMedicaController");

                return StatusCode(500, new ResponseAPI<List<CondicionMedicaReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los CondicionMedica.",
                    CantRegistros = 0
                });




            }
        }

        [HttpGet("MedioTransporte")]
        public async Task<ActionResult<ResponseAPI<List<MedioTransporteReaderDto>>>> GetMedioTransporteAsync()
        {
            try
            {
                var trasporte = await medioTransporteService.GetMedioTransporteAsync();
                if (trasporte == null || trasporte.Count == 0)
                    return NotFound(new ResponseAPI<List<MedioTransporteReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No de encontraron Medios de transporte ",
                        CantRegistros = 0
                    });
                return Ok(new ResponseAPI<List<MedioTransporteReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = trasporte,
                    Mensaje = "se encontraron Medios de transporte Correctamente ",
                    CantRegistros = trasporte.Count
                });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                   message: ex.Message,
                   StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                   ruta: $"/api/",
                   ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "MedioTransporteController");

                return StatusCode(500, new ResponseAPI<List<MedioTransporteReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los MedioTransporte.",
                    CantRegistros = 0
                });

            }
        }
        [HttpGet("TipoVivienda")]
        public async Task<ActionResult<ResponseAPI<List<TipoViviendaReaderDto>>>> GetMedioTipoViviendaAsync()
        {
            try
            {
                var vivienda = await tipoViviendaService.GetTipoViviendaAsync();
                if (vivienda == null || vivienda.Count == 0)
                    return NotFound(new ResponseAPI<List<TipoViviendaReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No de encontraron Tipos de vivienda ",
                        CantRegistros = 0
                    });
                return Ok(new ResponseAPI<List<TipoViviendaReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = vivienda,
                    Mensaje = "se encontraron tipos de Vivienda Correctamente ",
                    CantRegistros = vivienda.Count
                });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                   message: ex.Message,
                   StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                   ruta: $"/api/",
                   ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "TiposViviendaController");

                return StatusCode(500, new ResponseAPI<List<TipoViviendaReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los TipoVivienda.",
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("ClaseVivienda")]
        public async Task<ActionResult<ResponseAPI<List<ClaseViviendaReaderDto>>>> GetClaseViviendaAsync()
        {
            try
            {
                var clase = await claseViviendaService.GetClaseViviendaAsync();
                if (clase == null || clase.Count == 0)
                    return NotFound(new ResponseAPI<List<ClaseViviendaReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No de encontraron Clases de vivienda ",
                        CantRegistros = 0
                    });
                return Ok(new ResponseAPI<List<ClaseViviendaReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = clase,
                    Mensaje = "se encontraron Clases de Vivienda Correctamente ",
                    CantRegistros = clase.Count
                });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                   message: ex.Message,
                   StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                   ruta: $"/api/",
                   ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "ClasesViviendaController");

                return StatusCode(500, new ResponseAPI<List<ClaseViviendaReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los ClasesVivienda.",
                    CantRegistros = 0
                });
            }
        }
        [HttpGet("NivelAcademico")]
        public async Task<ActionResult<ResponseAPI<List<NivelAcademicoReaderDto>>>> GetNivelAcademicoAsync()
        {
            try
            {
                var nivel = await nivelAcademicoService.GetNivelAcademicosAsync();
                if (nivel == null || nivel.Count == 0)
                    return NotFound(new ResponseAPI<List<NivelAcademicoReaderDto>>
                    {
                        EsCorrecto = false,
                        Valor = null,
                        Mensaje = "No de encontraron  Nivel Academico ",
                        CantRegistros = 0
                    });
                return Ok(new ResponseAPI<List<NivelAcademicoReaderDto>>
                {
                    EsCorrecto = true,
                    Valor = nivel,
                    Mensaje = "se encontraron Clases de Vivienda Correctamente ",
                    CantRegistros = nivel.Count
                });
            }
            catch (Exception ex)
            {

                await _log.GuardarErrorAsync(
                   message: ex.Message,
                   StackTrace: ex.StackTrace,
                    usuario: User.Identity?.Name ?? "Sistema",
                    metodo: "HttpGet",
                   ruta: $"/api/",
                   ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    origen: "NivelAcademicoController");

                return StatusCode(500, new ResponseAPI<List<NivelAcademicoReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = null,
                    Mensaje = "Ocurrió un error al obtener los ClasesVivienda.",
                    CantRegistros = 0
                });

            }
        }
    }
}



















