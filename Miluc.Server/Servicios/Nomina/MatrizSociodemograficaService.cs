using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;

using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class MatrizSociodemograficaService(NominaDbContext _context) : IMatrizSociodemograficaService

    {


        public async Task<MatrizSociodemograficaReaderDto> GetMatrizPorEmpleadoIdAsync(int empleadoId)
        {
            try
            {
                var query = from e in _context.Empleado.AsNoTracking()
                            join m in _context.MatrizSociodemografica.AsNoTracking()
                                on e.EmpleadoId equals m.EmpleadoId into matrizGroup
                            from m in matrizGroup.DefaultIfEmpty()
                            where e.EmpleadoId == empleadoId
                            select new { e, m };

                var resultado = await query.Select(x => new MatrizSociodemograficaReaderDto
                {
                    EmpleadoId = x.e.EmpleadoId,
                    NombreEmpleado = $"{x.e.PrimerNombre} {x.e.SegundoNombre} {x.e.PrimerApellido} {x.e.SegundoApellido}"
                                        .Replace("  ", " ")
                                        .Trim(),

                    MatrizSociodemograficaId = x.m != null ? x.m.MatrizSociodemograficaId : 0,ConsentimientoInformado = x.m != null ? x.m.ConSentimientoInformado : false,
                    Edad = x.m != null ? x.m.Edad : 0,
                    NacionalidadId = x.m != null ? x.m.NacionalidadId : 0,
                    NombreNacionalidad = x.m != null && x.m.Nacionalidad != null? x.m.Nacionalidad.NacionalidadNombre: string.Empty,

                    PaisNacimientoId = x.m != null ? x.m.PaisNacimientoId : 0,NombrePais = x.m != null && x.m.Pais != null? x.m.Pais.pais:string.Empty,
                    GeneroId = x.m != null ? x.m.GeneroId : 0,NombreGenero = x.m != null && x.m.Genero != null? x.m.Genero.Nombre: string.Empty,
                    Fuma = x.m != null ? x.m.Fuma : false,FumaVecesAlMes = x.m != null ? x.m.FumaVecesAlMes : 0,
                    Alcohol = x.m != null ? x.m.Alcohol : false,
                    BebeVecesAlAño = x.m != null ? x.m.BebeVecesAlAño : 0,
                    DeporteId = x.m != null ? x.m.DeporteId : 0,
                    NombreDeporte = x.m != null && x.m.Deporte != null? x.m.Deporte.Nombre: string.Empty,
                    Hobbies = x.m != null ? x.m.Hobbies : string.Empty,
                    MedioDeTransporteId = x.m != null ? x.m.MedioTransporteId : 0,
                    NombreMedioDeTransporte = x.m != null && x.m.MedioTransporte != null? x.m.MedioTransporte.Nombre: string.Empty,
                    ClaseDeViviendaId = x.m != null ? x.m.ClaseDeViviendaId : 0,NombreClaseVivienda = x.m != null && x.m.ClaseVivienda != null? x.m.ClaseVivienda.Nombre: string.Empty,
                    TipoDeViviendaId = x.m != null ? x.m.TipoViviendaId : 0,NombreTipoDeVivienda = x.m != null && x.m.TipoVivienda != null? x.m.TipoVivienda.Nombre: string.Empty,
                    NivelAcademicoId = x.m != null ? x.m.NivelAcademicoId : 0,NombreNivelAcademico = x.m != null && x.m.NivelAcademico != null? x.m.NivelAcademico.Nombre: string.Empty,
                    AñoFinalizacionEducacion = x.m != null? x.m.AñoFinalizacionEducacion: null,
                    EntidadEducativa = x.m != null? x.m.EntidadEducativa: string.Empty,
                    TituloObtenido = x.m != null? x.m.TituloObtenido: string.Empty,UltimaEmpresaTrabajo = x.m != null? x.m.UltimaEmpresaTrabajo: string.Empty,
                    FechaUltimoEmpleo = x.m != null? x.m.FechaUltimoEmpleo: null,
                    CargoDesempeñado = x.m != null? x.m.CargoDesempeñado: string.Empty,
                    UltimoSalario = x.m != null? x.m.UltimoSalario: 0,
                    PersonaEnCasoDeEmergencia = x.m != null? x.m.PersonaEnCasoDeEmergencia: string.Empty,
                    TelefonoEmergencia = x.m != null? x.m.TelefonoEmergencia: string.Empty,
                    DireccionPersonaEmergencia = x.m != null? x.m.DireccionPersonaEmergencia: string.Empty,
                    FechaCreacion = x.m != null? x.m.FechaCreacion: null,
                    FechaActualizacion = x.m != null? x.m.FechaActualizacion: null,
                    Activo = x.m != null? x.m.Activo                        : false
                })
                .FirstOrDefaultAsync();

                if (resultado != null && resultado.MatrizSociodemograficaId > 0)
                {
                    resultado.CondicionesMedicas =
                        await _context.CondicionMedicaDetalle
                            .AsNoTracking().Where(x =>
                                x.MatrizSociodemograficaId ==
                                resultado.MatrizSociodemograficaId)
                            .Include(x => x.CondicionMedica)
                            .Select(x => new CondicionMedicaDetalleReaderDto
                            {
                                CondicionMedicaId = x.CondicionMedicaId,
                                NombreCondicionMedica = x.CondicionMedica.Nombre,
                                OtraCondicionMedica = x.OtraCondicionMedica
                            })
                            .ToListAsync();
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error al obtener la matriz sociodemográfica: {ex.Message}",
                    ex);
            }
        }


        public async Task<(List<MatrizSociodemograficaReaderDto> data, int CantidadRegistros)>GetMatrizSocioDemograficasAsync(string? filtro = null,int page = 1,int? cantidad = null,string? correo = null,int? idEmpresa = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                var query = from e in _context.Empleado.AsNoTracking()
                            join m in _context.MatrizSociodemografica
                                on e.EmpleadoId equals m.EmpleadoId into matrizGroup
                            from m in matrizGroup.DefaultIfEmpty()
                            where string.IsNullOrEmpty(filtro) ||
                                  e.PrimerNombre.Contains(filtro) ||
                                  e.PrimerApellido.Contains(filtro)
                            select new { e, m };

                if (!string.IsNullOrEmpty(correo))
                {
                    query = query.Where(x => x.e.CorreoElectronico == correo);
                }

                if (idEmpresa.HasValue && idEmpresa.Value > 0)
                {
                    query = query.Where(x => x.e.EmpresaId == idEmpresa.Value);
                }

                var totalRegistros = await query.CountAsync();

                var matriz = await query
                    .Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .Select(x => new MatrizSociodemograficaReaderDto
                    {
                        EmpleadoId = x.e.EmpleadoId,
                        NombreEmpleado = $"{x.e.PrimerNombre} {x.e.SegundoNombre} {x.e.PrimerApellido} {x.e.SegundoApellido}",

                        MatrizSociodemograficaId = x.m != null ? x.m.MatrizSociodemograficaId : 0,

                        NacionalidadId = x.m != null ? x.m.NacionalidadId : 0,
                        NombreNacionalidad = x.m != null && x.m.Nacionalidad != null? x.m.Nacionalidad.NacionalidadNombre: string.Empty,
                        PaisNacimientoId = x.m != null ? x.m.PaisNacimientoId : 0,
                        NombrePais = x.m != null && x.m.Pais != null? x.m.Pais.pais: string.Empty,
                        GeneroId = x.m != null ? x.m.GeneroId : 0,NombreGenero = x.m != null && x.m.Genero != null? x.m.Genero.Nombre: string.Empty,
                        ConsentimientoInformado = x.m != null? x.m.ConSentimientoInformado: false,
                        Edad = x.m != null ? x.m.Edad : 0,
                        Fuma = x.m != null ? x.m.Fuma : false,
                        FumaVecesAlMes = x.m != null ? x.m.FumaVecesAlMes : 0,
                        Alcohol = x.m != null ? x.m.Alcohol : false,
                        BebeVecesAlAño = x.m != null ? x.m.BebeVecesAlAño : 0,
                        DeporteId = x.m != null ? x.m.DeporteId : 0,NombreDeporte = x.m != null && x.m.Deporte != null? x.m.Deporte.Nombre: string.Empty,
                        Hobbies = x.m != null? x.m.Hobbies: string.Empty,
                        MedioDeTransporteId = x.m != null? x.m.MedioTransporteId: 0,
                        NombreMedioDeTransporte = x.m != null &&x.m.MedioTransporte != null? x.m.MedioTransporte.Nombre: string.Empty,
                        ClaseDeViviendaId = x.m != null? x.m.ClaseDeViviendaId: 0,
                        NombreClaseVivienda = x.m != null &&x.m.ClaseVivienda != null? x.m.ClaseVivienda.Nombre: string.Empty,
                        TipoDeViviendaId = x.m != null? x.m.TipoViviendaId: 0,
                        NombreTipoDeVivienda = x.m != null &&x.m.TipoVivienda != null? x.m.TipoVivienda.Nombre: string.Empty,
                        NivelAcademicoId = x.m != null? x.m.NivelAcademicoId: 0,
                        NombreNivelAcademico = x.m != null &&x.m.NivelAcademico != null? x.m.NivelAcademico.Nombre: string.Empty,
                        AñoFinalizacionEducacion = x.m != null? x.m.AñoFinalizacionEducacion: null,         
                        EntidadEducativa = x.m != null? x.m.EntidadEducativa: string.Empty,
                        TituloObtenido = x.m != null? x.m.TituloObtenido: string.Empty,UltimaEmpresaTrabajo = x.m != null? x.m.UltimaEmpresaTrabajo: string.Empty,
                        FechaUltimoEmpleo = x.m != null? x.m.FechaUltimoEmpleo: null,CargoDesempeñado = x.m != null? x.m.CargoDesempeñado: string.Empty,
                        UltimoSalario = x.m != null? x.m.UltimoSalario: 0,
                        PersonaEnCasoDeEmergencia = x.m != null? x.m.PersonaEnCasoDeEmergencia: string.Empty,
                        TelefonoEmergencia = x.m != null? x.m.TelefonoEmergencia: string.Empty,DireccionPersonaEmergencia = x.m != null? x.m.DireccionPersonaEmergencia: string.Empty,
                        FechaCreacion = x.m != null? x.m.FechaCreacion: null,
                        FechaActualizacion = x.m != null? x.m.FechaActualizacion: null,
                        Activo = x.m != null? x.m.Activo: false
                    })
                    .ToListAsync();

                var idsMatrices = matriz
                    .Where(x => x.MatrizSociodemograficaId > 0)
                    .Select(x => x.MatrizSociodemograficaId)
                    .ToList();

                var condiciones = await _context.CondicionMedicaDetalle
                    .AsNoTracking()
                    .Include(x => x.CondicionMedica)
                    .Where(x => idsMatrices.Contains(x.MatrizSociodemograficaId))
                    .ToListAsync();

                foreach (var item in matriz)
                {
                    item.CondicionesMedicas = condiciones
                        .Where(x => x.MatrizSociodemograficaId == item.MatrizSociodemograficaId)
                        .Select(x => new CondicionMedicaDetalleReaderDto
                        {
                            CondicionMedicaId = x.CondicionMedicaId,
                            NombreCondicionMedica = x.CondicionMedica.Nombre,
                            OtraCondicionMedica = x.OtraCondicionMedica
                        })
                        .ToList();
                }

                return (matriz, totalRegistros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
        public async Task<bool> updateMatrizAsync(MatrizSocioDemograficaUpdateDto dto)
        {
            try
            {
                var matrizExistente = await _context.MatrizSociodemografica
                    .Include(x => x.Empleado)
                    .Include(x => x.CondicionesMedicas)
                    .FirstOrDefaultAsync(x =>
                        x.EmpleadoId == dto.EmpleadoId ||
                        x.MatrizSociodemograficaId == dto.MatrizSociodemograficaId);

                // Crear
                if (matrizExistente == null)
                {
                    var nuevaMatriz = new MatrizSociodemografica
                    {
                        EmpleadoId = dto.EmpleadoId,
                        ConSentimientoInformado = dto.ConsentimientoInformado,
                        Edad = dto.Edad,
                        NacionalidadId = dto.NacionalidadId,
                        PaisNacimientoId = dto.PaisNacimientoId,
                        GeneroId = dto.GeneroId,
                        Fuma = dto.Fuma ?? false,
                        FumaVecesAlMes = dto.FumaVecesAlMes,
                        Alcohol = dto.Alcohol ?? false,
                        BebeVecesAlAño = dto.BebeVecesAlAño,
                        DeporteId = dto.DeporteId,
                        Hobbies = dto.Hobbies.Trim().ToUpper(),
                        MedioTransporteId = dto.MedioDeTransporteId,
                        ClaseDeViviendaId = dto.ClaseDeViviendaId,
                        TipoViviendaId = dto.TipoDeViviendaId,
                        NivelAcademicoId = dto.NivelAcademicoId,
                        EntidadEducativa = dto.EntidadEducativa.Trim().ToUpper(),
                        TituloObtenido = dto.TituloObtenido.Trim().ToUpper(),
                        UltimaEmpresaTrabajo = dto.UltimaEmpresaTrabajo.Trim().ToUpper(),
                        FechaUltimoEmpleo = dto.FechaUltimoEmpleo,
                        CargoDesempeñado = dto.CargoDesempeñado.Trim().ToUpper(),
                        UltimoSalario = dto.UltimoSalario ?? 0,
                        PersonaEnCasoDeEmergencia = dto.PersonaEnCasoDeEmergencia.Trim().ToUpper(),
                        TelefonoEmergencia = dto.TelefonoEmergencia,
                        DireccionPersonaEmergencia = dto.DireccionPersonaEmergencia.Trim().ToUpper(),
                        AñoFinalizacionEducacion = dto.AñoFinalizacionEducacion,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Activo = true
                    };

                    await _context.MatrizSociodemografica.AddAsync(nuevaMatriz);

                    // Necesitamos el Id de la matriz
                    await _context.SaveChangesAsync();

                    foreach (var condicionId in dto.CondicionesMedicasIds)
                    {
                        await _context.CondicionMedicaDetalle.AddAsync(
                            new CondicionMedicaDetalle
                            {
                                MatrizSociodemograficaId = nuevaMatriz.MatrizSociodemograficaId,
                                CondicionMedicaId = condicionId,

                               
                                OtraCondicionMedica = condicionId == 22
                                    ? dto.OtraCondicionMedica
                                    : null
                            });
                    }
                }
                else
                {
                    matrizExistente.ConSentimientoInformado = dto.ConsentimientoInformado;
                    matrizExistente.Edad = dto.Edad;
                    matrizExistente.NacionalidadId = dto.NacionalidadId;
                    matrizExistente.PaisNacimientoId = dto.PaisNacimientoId;
                    matrizExistente.GeneroId = dto.GeneroId;
                    matrizExistente.Fuma = dto.Fuma ?? false;
                    matrizExistente.FumaVecesAlMes = dto.FumaVecesAlMes;
                    matrizExistente.Alcohol = dto.Alcohol ?? false;
                    matrizExistente.BebeVecesAlAño = dto.BebeVecesAlAño;
                    matrizExistente.DeporteId = dto.DeporteId;
                    matrizExistente.Hobbies = dto.Hobbies.Trim().ToUpper();
                    matrizExistente.MedioTransporteId = dto.MedioDeTransporteId;
                    matrizExistente.ClaseDeViviendaId = dto.ClaseDeViviendaId;
                    matrizExistente.TipoViviendaId = dto.TipoDeViviendaId;
                    matrizExistente.NivelAcademicoId = dto.NivelAcademicoId;
                    matrizExistente.AñoFinalizacionEducacion = dto.AñoFinalizacionEducacion;
                    matrizExistente.EntidadEducativa = dto.EntidadEducativa.Trim().ToUpper();
                    matrizExistente.TituloObtenido = dto.TituloObtenido.Trim().ToUpper();
                    matrizExistente.UltimaEmpresaTrabajo = dto.UltimaEmpresaTrabajo.Trim().ToUpper();
                    matrizExistente.FechaUltimoEmpleo = dto.FechaUltimoEmpleo;
                    matrizExistente.CargoDesempeñado = dto.CargoDesempeñado.Trim().ToUpper();
                    matrizExistente.UltimoSalario = dto.UltimoSalario ?? 0;
                    matrizExistente.PersonaEnCasoDeEmergencia = dto.PersonaEnCasoDeEmergencia.Trim().ToUpper();
                    matrizExistente.TelefonoEmergencia = dto.TelefonoEmergencia;
                    matrizExistente.DireccionPersonaEmergencia = dto.DireccionPersonaEmergencia.Trim().ToUpper();
                    matrizExistente.FechaActualizacion = DateTime.Now;

                    // Eliminar condiciones existentes
                    var condicionesActuales = await _context.CondicionMedicaDetalle
                        .Where(x => x.MatrizSociodemograficaId ==
                                    matrizExistente.MatrizSociodemograficaId)
                        .ToListAsync();

                    _context.CondicionMedicaDetalle.RemoveRange(condicionesActuales);

                    if (!dto.CondicionesMedicasIds.Contains(22))
                    {
                        dto.OtraCondicionMedica = null;
                    }
                    // Insertar las nuevas
                    foreach (var condicionId in dto.CondicionesMedicasIds)
                    {
                        await _context.CondicionMedicaDetalle.AddAsync(
                            new CondicionMedicaDetalle
                            {
                                MatrizSociodemograficaId =
                                    matrizExistente.MatrizSociodemograficaId,

                                CondicionMedicaId = condicionId,

                                // Reemplaza 999 por el Id real de "Otro"
                                OtraCondicionMedica = condicionId == 22
                                    ? dto.OtraCondicionMedica
                                    : null
                            });
                    }
                }

                var resultado = await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al crear o actualizar la MatrizSociodemografica",
                    ex);
            }
        }
    }
    }

