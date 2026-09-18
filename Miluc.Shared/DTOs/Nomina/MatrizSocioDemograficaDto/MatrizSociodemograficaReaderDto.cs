using Miluc.Shared.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto
{
    // 1. Corregido el nombre de la clase (se removió la 'd' inicial)
    public class MatrizSociodemograficaReaderDto
    {
        public bool ConsentimientoInformado { get; set; } // Anulable si no hay matriz

        public int MatrizSociodemograficaId { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        
        public int Edad { get; set; }
        public string Nacionalidad { get; set; } = string.Empty;
        public int PaisNacimientoId { get; set; }
        public string NombrePais { get; set; } = string.Empty;
        public int GeneroId { get; set; }
        public string NombreGenero { get; set; } = string.Empty;
        public bool? Fuma { get; set; }
        public int FumaVecesAlMes { get; set; }
        public bool? Alcohol { get; set; } // Anulable si no hay matriz
        public int BebeVecesAlAño { get; set; }
        public int DeporteId { get; set; }
        public string NombreDeporte { get; set; } = string.Empty;
        public int CondicionMedicaId { get; set; }
        public string NombreCondicionMedica { get; set; } = string.Empty;
        public string Hobbies { get; set; } = string.Empty;
        public int MedioDeTransporteId { get; set; }
        public string NombreMedioDeTransporte { get; set; } = string.Empty;
        public int ClaseDeViviendaId { get; set; }
        public string NombreClaseVivienda { get; set; } = string.Empty;
        public int TipoDeViviendaId { get; set; }
        public string NombreTipoDeVivienda { get; set; } = string.Empty;
        public int NivelAcademicoId { get; set; }
        public string NombreNivelAcademico { get; set; } = string.Empty;
        public DateTime? AñoFinalizacionEducacion { get; set; }
        public string EntidadEducativa { get; set; } = string.Empty;

        public string TituloObtenido { get; set; } = string.Empty;
        public string UltimaEmpresaTrabajo { get; set; } = string.Empty;

        // Fechas correctamente anulables
        public DateTime? FechaUltimoEmpleo { get; set; }

        public string CargoDesempeñado { get; set; } = string.Empty;
        public decimal UltimoSalario { get; set; }
        public string PersonaEnCasoDeEmergencia { get; set; } = string.Empty;
        public string TelefonoEmergencia { get; set; } = string.Empty;
        public string DireccionPersonaEmergencia { get; set; } = string.Empty;

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool? Activo { get; set; }
    }
}