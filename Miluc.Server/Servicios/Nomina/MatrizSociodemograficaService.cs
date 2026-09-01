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
                    // --- Datos del Empleado (Siempre seguros) ---
                    EmpleadoId = x.e.EmpleadoId,
                    NombreEmpleado = $"{x.e.PrimerNombre} {x.e.SegundoNombre} {x.e.PrimerApellido} {x.e.SegundoApellido}".Replace("  ", " ").Trim(),

                    // --- Datos de la Matriz (Si x.m es null, se asignan valores por defecto) ---
                    MatrizSociodemograficaId = x.m != null ? x.m.MatrizSociodemograficaId : 0,
                    ConsentimientoInformado = x.m != null ? x.m.ConSentimientoInformado : false,
                    Edad = x.m != null ? x.m.Edad : 0,
                    Nacionalidad = x.m != null ? x.m.Nacionalidad : "Sin asignar",
                    PaisNacimientoId = x.m != null ? x.m.PaisNacimientoId : 0,
                    NombrePais = x.m != null && x.m.Pais != null ? x.m.Pais.pais : "Sin asignar",
                    GeneroId = x.m != null ? x.m.GeneroId : 0,
                    NombreGenero = x.m != null && x.m.Genero != null ? x.m.Genero.Nombre : "Sin asignar",
                    Fuma = x.m != null ? x.m.Fuma : false,
                    FumaVecesAlMes = x.m != null ? x.m.FumaVecesAlMes : 0,
                    Alcohol = x.m != null ? x.m.Alcohol : false,
                    BebeVecesAlAño = x.m != null ? x.m.BebeVecesAlAño : 0,
                    DeporteId = x.m != null ? x.m.DeporteId : 0,
                    NombreDeporte = x.m != null && x.m.Deporte != null ? x.m.Deporte.Nombre : "Sin asignar",
                    CondicionMedicaId = x.m != null ? x.m.CondicionMedicaId : 0,
                    NombreCondicionMedica = x.m != null && x.m.CondicionMedica != null ? x.m.CondicionMedica.Nombre : "Sin asignar",
                    Hobbies = x.m != null ? x.m.Hobbies : "Sin asignar",
                    MedioDeTransporteId = x.m != null ? x.m.MedioTransporteId : 0,
                    NombreMedioDeTransporte = x.m != null && x.m.MedioTransporte != null ? x.m.MedioTransporte.Nombre : "Sin asignar",
                    ClaseDeViviendaId = x.m != null ? x.m.ClaseDeViviendaId : 0,
                    NombreClaseVivienda = x.m != null && x.m.ClaseVivienda != null ? x.m.ClaseVivienda.Nombre : "Sin asignar",
                    TipoDeViviendaId = x.m != null ? x.m.TipoViviendaId : 0,
                    NombreTipoDeVivienda = x.m != null && x.m.TipoVivienda != null ? x.m.TipoVivienda.Nombre : "Sin asignar",
                    NivelAcademicoId = x.m != null ? x.m.NivelAcademicoId : 0,
                    NombreNivelAcademico = x.m != null && x.m.NivelAcademico != null ? x.m.NivelAcademico.Nombre : "Sin asignar",
                    AñoFinalizacionEducacion = x.m != null ? x.m.AñoFinalizacionEducacion : null,
                    EntidadEducativa = x.m != null ? x.m.EntidadEducativa : "Sin asignar",
                    TituloObtenido = x.m != null ? x.m.TituloObtenido : "Sin asignar",
                    UltimaEmpresaTrabajo = x.m != null ? x.m.UltimaEmpresaTrabajo : "Sin asignar",
                    FechaUltimoEmpleo = x.m != null ? x.m.FechaUltimoEmpleo : null,
                    CargoDesempeñado = x.m != null ? x.m.CargoDesempeñado : "Sin asignar",
                    UltimoSalario = x.m != null ? x.m.UltimoSalario : 0,
                    PersonaEnCasoDeEmergencia = x.m != null ? x.m.PersonaEnCasoDeEmergencia : "Sin asignar",
                    TelefonoEmergencia = x.m != null ? x.m.TelefonoEmergencia : "Sin asignar",
                    DireccionPersonaEmergencia = x.m != null ? x.m.DireccionPersonaEmergencia : "Sin asignar",
                    FechaCreacion = x.m != null ? x.m.FechaCreacion : null,
                    FechaActualizacion = x.m != null ? x.m.FechaActualizacion : null,
                    Activo = x.m != null ? x.m.Activo : false
                })
                .FirstOrDefaultAsync();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener la matriz sociodemográfica: {ex.Message}", ex);
            }
        }


        public async Task<(List<MatrizSociodemograficaReaderDto> data, int CantidadRegistros)> GetMatrizSocioDemograficasAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                // 1. Iniciamos desde Empleados para traerlos a todos
                var query = from e in _context.Empleado.AsNoTracking()
                            // 2. Hacemos un LEFT JOIN con MatrizSociodemografica
                            join m in _context.MatrizSociodemografica
                                 on e.EmpleadoId equals m.EmpleadoId into matrizGroup
                            from m in matrizGroup.DefaultIfEmpty()
                                // 3. Aplicamos el filtro sobre el nombre del empleado
                            where string.IsNullOrEmpty(filtro) ||
                                  e.PrimerNombre.Contains(filtro) ||
                                  e.PrimerApellido.Contains(filtro)
                            select new { e, m }; // Proyectamos temporalmente (e = Empleado, m = Matriz)

                var totalRegistros = await query.CountAsync();

                var matriz = await query
                    .Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .Select(x => new MatrizSociodemograficaReaderDto
                    {
                        // -- DATOS DEL EMPLEADO (Siempre existen) --
                        EmpleadoId = x.e.EmpleadoId,
                        NombreEmpleado = $"{x.e.PrimerNombre} {x.e.SegundoNombre} {x.e.PrimerApellido} {x.e.SegundoApellido}",

                        // -- DATOS DE LA MATRIZ --
                        // Si x.m es null, significa que no tiene matriz. Asignamos ID 0.
                        MatrizSociodemograficaId = x.m != null ? x.m.MatrizSociodemograficaId : 0,

                        // Textos: Si no hay matriz, devolvemos "Sin asignar"
                        Nacionalidad = x.m != null ? x.m.Nacionalidad : "Sin asignar",
                        NombrePais = x.m != null && x.m.Pais != null ? x.m.Pais.pais : "Sin asignar",
                        NombreGenero = x.m != null && x.m.Genero != null ? x.m.Genero.Nombre : "Sin asignar",
                        NombreDeporte = x.m != null && x.m.Deporte != null ? x.m.Deporte.Nombre : "Sin asignar",
                        NombreCondicionMedica = x.m != null && x.m.CondicionMedica != null ? x.m.CondicionMedica.Nombre : "Sin asignar",
                        Hobbies = x.m != null && !string.IsNullOrEmpty(x.m.Hobbies) ? x.m.Hobbies : "Sin asignar",
                        NombreMedioDeTransporte = x.m != null && x.m.MedioTransporte != null ? x.m.MedioTransporte.Nombre : "Sin asignar",
                        NombreClaseVivienda = x.m != null && x.m.ClaseVivienda != null ? x.m.ClaseVivienda.Nombre : "Sin asignar",
                        NombreTipoDeVivienda = x.m != null && x.m.TipoVivienda != null ? x.m.TipoVivienda.Nombre : "Sin asignar",
                        NombreNivelAcademico = x.m != null && x.m.NivelAcademico != null ? x.m.NivelAcademico.Nombre : "Sin asignar",
                        EntidadEducativa = x.m != null && !string.IsNullOrEmpty(x.m.EntidadEducativa) ? x.m.EntidadEducativa : "Sin asignar",
                        TituloObtenido = x.m != null && !string.IsNullOrEmpty(x.m.TituloObtenido) ? x.m.TituloObtenido : "Sin asignar",
                        UltimaEmpresaTrabajo = x.m != null && !string.IsNullOrEmpty(x.m.UltimaEmpresaTrabajo) ? x.m.UltimaEmpresaTrabajo : "Sin asignar",
                        CargoDesempeñado = x.m != null && !string.IsNullOrEmpty(x.m.CargoDesempeñado) ? x.m.CargoDesempeñado : "Sin asignar",
                        PersonaEnCasoDeEmergencia = x.m != null && !string.IsNullOrEmpty(x.m.PersonaEnCasoDeEmergencia) ? x.m.PersonaEnCasoDeEmergencia : "Sin asignar",
                        TelefonoEmergencia = x.m != null && !string.IsNullOrEmpty(x.m.TelefonoEmergencia) ? x.m.TelefonoEmergencia : "Sin asignar",
                        DireccionPersonaEmergencia = x.m != null && !string.IsNullOrEmpty(x.m.DireccionPersonaEmergencia) ? x.m.DireccionPersonaEmergencia : "Sin asignar",

                        // Valores numéricos o booleanos (Asignamos 0 o false si no hay matriz)
                        ConsentimientoInformado = x.m != null ? x.m.ConSentimientoInformado : false,
                        Edad = x.m != null ? x.m.Edad : 0,
                        PaisNacimientoId = x.m != null ? x.m.PaisNacimientoId : 0,
                        GeneroId = x.m != null ? x.m.GeneroId : 0,
                        Fuma = x.m != null ? x.m.Fuma : false,
                        FumaVecesAlMes = x.m != null ? x.m.FumaVecesAlMes : 0,
                        Alcohol = x.m != null ? x.m.Alcohol : false,
                        BebeVecesAlAño = x.m != null ? x.m.BebeVecesAlAño : 0,
                        DeporteId = x.m != null ? x.m.DeporteId : 0,
                        CondicionMedicaId = x.m != null ? x.m.CondicionMedicaId : 0,
                        MedioDeTransporteId = x.m != null ? x.m.MedioTransporteId : 0,
                        ClaseDeViviendaId = x.m != null ? x.m.ClaseDeViviendaId : 0,
                        TipoDeViviendaId = x.m != null ? x.m.TipoViviendaId : 0,
                        NivelAcademicoId = x.m != null ? x.m.NivelAcademicoId : 0,
                        AñoFinalizacionEducacion = x.m != null ? x.m.AñoFinalizacionEducacion : null,
                        UltimoSalario = x.m != null ? x.m.UltimoSalario : 0,

                        // Fechas (Deben ser anulables en tu DTO: DateTime?)
                        FechaUltimoEmpleo = x.m != null ? x.m.FechaUltimoEmpleo : null,
                        FechaCreacion = x.m != null ? x.m.FechaCreacion : null,
                        FechaActualizacion = x.m != null ? x.m.FechaActualizacion : null,
                        Activo = x.m != null ? x.m.Activo : false

                    })
                    .ToListAsync();

                return (matriz, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la MatrizSocioDemografica", ex);
            }
        }
        public async Task<bool> updateMatrizAsync(MatrizSocioDemograficaUpdateDto dto)
        {
            try
            {
                // Buscar si ya existe una matriz para el empleado por matrizId o por empleadoId
                var matrizExistente = await _context.MatrizSociodemografica
                    .Include(x => x.Empleado)
                    .FirstOrDefaultAsync(x => x.EmpleadoId == dto.EmpleadoId || x.MatrizSociodemograficaId == dto.MatrizSociodemograficaId);

                // Si no existe, crear una nueva matriz
                if (matrizExistente == null)
                {
                    var nuevaMatriz = new MatrizSociodemografica
                    {
                        EmpleadoId = dto.EmpleadoId,
                        ConSentimientoInformado = dto.ConsentimientoInformado,
                        Edad = dto.Edad,
                        Nacionalidad = dto.Nacionalidad,
                        PaisNacimientoId = dto.PaisNacimientoId,
                        GeneroId = dto.GeneroId,
                        Fuma = dto.Fuma ?? false,
                        FumaVecesAlMes = dto.FumaVecesAlMes,
                        Alcohol = dto.Alcohol ?? false,
                        BebeVecesAlAño = dto.BebeVecesAlAño,
                        DeporteId = dto.DeporteId,
                        CondicionMedicaId = dto.CondicionMedicaId,
                        Hobbies = dto.Hobbies,
                        MedioTransporteId = dto.MedioDeTransporteId,
                        ClaseDeViviendaId = dto.ClaseDeViviendaId,
                        TipoViviendaId = dto.TipoDeViviendaId,
                        NivelAcademicoId = dto.NivelAcademicoId,
                        EntidadEducativa = dto.EntidadEducativa,
                        TituloObtenido = dto.TituloObtenido,
                        UltimaEmpresaTrabajo = dto.UltimaEmpresaTrabajo,
                        FechaUltimoEmpleo = dto.FechaUltimoEmpleo,
                        CargoDesempeñado = dto.CargoDesempeñado,
                        UltimoSalario = dto.UltimoSalario,
                        PersonaEnCasoDeEmergencia = dto.PersonaEnCasoDeEmergencia,
                        TelefonoEmergencia = dto.TelefonoEmergencia,
                        DireccionPersonaEmergencia = dto.DireccionPersonaEmergencia,

                        // Extrae el entero del año si la fecha no es nula
                        AñoFinalizacionEducacion = dto.AñoFinalizacionEducacion,

                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Activo = true
                    };

                    await _context.MatrizSociodemografica.AddAsync(nuevaMatriz);
                }
                else
                {
                    // En caso de que ya exista, actualizamos
                    matrizExistente.ConSentimientoInformado = dto.ConsentimientoInformado;
                    matrizExistente.Edad = dto.Edad;
                    matrizExistente.Nacionalidad = dto.Nacionalidad;
                    matrizExistente.PaisNacimientoId = dto.PaisNacimientoId;
                    matrizExistente.GeneroId = dto.GeneroId;
                    matrizExistente.Fuma = dto.Fuma ?? false;
                    matrizExistente.FumaVecesAlMes = dto.FumaVecesAlMes;
                    matrizExistente.Alcohol = dto.Alcohol ?? false;
                    matrizExistente.BebeVecesAlAño = dto.BebeVecesAlAño;
                    matrizExistente.DeporteId = dto.DeporteId;
                    matrizExistente.CondicionMedicaId = dto.CondicionMedicaId;
                    matrizExistente.Hobbies = dto.Hobbies;
                    matrizExistente.MedioTransporteId = dto.MedioDeTransporteId;
                    matrizExistente.ClaseDeViviendaId = dto.ClaseDeViviendaId;
                    matrizExistente.TipoViviendaId = dto.TipoDeViviendaId;
                    matrizExistente.NivelAcademicoId = dto.NivelAcademicoId;

                    // Extrae el entero del año o asigna 0 si viene nulo
                    matrizExistente.AñoFinalizacionEducacion = dto.AñoFinalizacionEducacion;

                    matrizExistente.EntidadEducativa = dto.EntidadEducativa;
                    matrizExistente.TituloObtenido = dto.TituloObtenido;
                    matrizExistente.UltimaEmpresaTrabajo = dto.UltimaEmpresaTrabajo;
                    matrizExistente.FechaUltimoEmpleo = dto.FechaUltimoEmpleo;
                    matrizExistente.CargoDesempeñado = dto.CargoDesempeñado;
                    matrizExistente.UltimoSalario = dto.UltimoSalario;
                    matrizExistente.PersonaEnCasoDeEmergencia = dto.PersonaEnCasoDeEmergencia;
                    matrizExistente.TelefonoEmergencia = dto.TelefonoEmergencia;
                    matrizExistente.DireccionPersonaEmergencia = dto.DireccionPersonaEmergencia;
                    matrizExistente.FechaActualizacion = DateTime.Now;
                }

                var resultado = await _context.SaveChangesAsync();
                return resultado >= 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear o actualizar la MatrizSociodemografica", ex);
            }
        }
    }
}


