using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class MatrizSociodemograficaService(NominaDbContext _context) : IMatrizSociodemograficaService

    {     

        public  async Task<(List<MatrizSociodemograficaReaderDto> data, int CantidadRegistros)> GetMatrizSocioDemograficasAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;

                var query = _context.MatrizSociodemografica.AsNoTracking().AsQueryable();
                if(!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(x => x.Empleado.PrimerNombre.Contains(filtro) || x.Empleado.PrimerApellido.Contains(filtro));
                }
                var totalRegistros = query.Count();
                var matriz = await query
                    .Include(x => x.Empleado) 
                    .Include(x => x.Pais)
                    .Include(x => x.Genero)
                    .Include(x => x.Deporte)
                    .Include(x => x.CondicionMedica)
                    .Include(x => x.MedioTransporte)
                    .Include(x => x.ClaseVivienda)
                    .Include(x => x.TipoVivienda)
                    .Include(x => x.NivelAcademico)
                    .Where(x => string.IsNullOrEmpty(filtro) || x.Empleado.PrimerNombre.Contains(filtro) || x.Empleado.PrimerApellido.Contains(filtro))
                    .Select(x => new MatrizSociodemograficaReaderDto
                    {
                        MatrizSociodemograficaID = x.MatrizSociodemograficaID,
                        EmpleadoId = x.EmpleadoId,
                        NombreEmpleado = $"{x.Empleado.PrimerNombre} {x.Empleado.SegundoNombre} {x.Empleado.PrimerApellido} {x.Empleado.SegundoApellido}",

                        ConsentimientoInformado = x.ConSentimientoInformado,
                        Edad = x.Edad,
                        Nacionalidad = x.Nacionalidad,

                        PaisNacimientoId = x.PaisNacimientoId,
                        NombrePais = x.Pais.pais,

                        GeneroId = x.GeneroId,
                        NombreGenero = x.Genero.Nombre,

                        Fuma = x.Fuma,
                        FumaVecesAlMes = x.FumaVecesAlMes,

                        Alcohol = x.Alcohol,
                        BebeVecesAlAño = x.BebeVecesAlAño,

                        DeporteId = x.DeporteId,
                        NombreDeporte = x.Deporte.Nombre,

                        CondicionMedicaId = x.CondicionMedicaId,
                        NombreCondicionMedica = x.CondicionMedica.Nombre,

                        Hobbies = x.Hobbies,

                        MedioDeTransporteId = x.MedioTransporteId,
                        NombreMedioDeTransporte = x.MedioTransporte.Nombre,

                        ClaseDeViviendaId = x.ClaseDeViviendaId,
                        NombreClaseVivienda = x.ClaseVivienda.Nombre,

                        TipoDeViviendaId = x.TipoViviendaId,
                        NombreTipoDeVivienda = x.TipoVivienda.Nombre,

                        NivelAcademicoId = x.NivelAcademicoId,
                        NombreNivelAcademico = x.NivelAcademico.Nombre,

                        AñoFinalizacionEducacion = x.AñoFinalizacionEducacion,
                        EntidadEducativa = x.EntidadEducativa,

                        TituloObtenido = x.TituloObtenido,
                        UltimaEmpresaTrabajo = x.UltimaEmpresaTrabajo,
                        FechaUltimoEmpleo = x.FechaUltimoEmpleo,

                        CargoDesempeñado = x.CargoDesempeñado,
                       
                        UltimoSalario = x.UltimoSalario,

                        PersonaEnCasoDeEmergencia = x.PersonaEnCasoDeEmergencia,
                        TelefonoEmergencia = x.TelefonoEmergencia,
                        DireccionPersonaEmergencia = x.DireccionPersonaEmergencia,

                        FechaCreacion = x.FechaCreacion,
                        FechaActualizacion = x.FechaActualizacion,
                        Activo = x.Activo

                    }).Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .ToListAsync();

                return (matriz, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener a,MatrizSocioDemografica", ex);
            }
        }
    }
}

