using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Timers;

namespace Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto
{
    public class MatrizSocioDemograficaUpdateDto
    {
        public bool ConsentimientoInformado { get; set; } // Anulable si no hay matriz

        public int MatrizSociodemograficaId { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
       

            
        public int Edad { get; set; }
        [Required(ErrorMessage = "El campo nacionalidad es obligatorio")]
        public string Nacionalidad { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un país de nacimiento")]


        public int PaisNacimientoId { get; set; }
        

        public string NombrePais { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un género")]
       
        public int GeneroId { get; set; }
       
        public string NombreGenero { get; set; } = string.Empty;

        public bool? Fuma { get; set; }
        public int FumaVecesAlMes { get; set; }
        public bool? Alcohol { get; set; } // Anulable si no hay matriz
        public int BebeVecesAlAño { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un deporte")]  
        public int DeporteId { get; set; }
        public string NombreDeporte { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar condicion medica")]
        public int CondicionMedicaId { get; set; }
        public string NombreCondicionMedica { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Hobbies es obligatorio")]
        public string Hobbies { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage =   "Debe seleccionar un medio de transporte")]
        public int MedioDeTransporteId { get; set; }
        public string NombreMedioDeTransporte { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar clase de vivienda")]
        public int ClaseDeViviendaId { get; set; }
        public string NombreClaseVivienda { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar tipo de vivienda")]
        public int TipoDeViviendaId { get; set; }
        public string NombreTipoDeVivienda { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar nivel academico")]
        public int NivelAcademicoId { get; set; }
        public string NombreNivelAcademico { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo fecha de finalización de estudios es obligatorio")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "La fecha de culminación de estudios fecha pasada o presente")]
        public DateTime? AñoFinalizacionEducacion { get; set; }
        [Required(ErrorMessage = "El campo entidad educativa es obligatorio")]
        public string EntidadEducativa { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo titulo obtenido es obligatorio")]
        public string TituloObtenido { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo última empresa es obligatorio")]
        public string UltimaEmpresaTrabajo { get; set; } = string.Empty;

        // Fechas correctamente anulables
        [Required(ErrorMessage = "El campo fecha de último empleo es obligatorio")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "La fecha de último empleo debe ser una fecha pasada o presente")]
        public DateTime? FechaUltimoEmpleo { get; set; }
        [Required(ErrorMessage = "El campo Cargo desempeñado es obligatorio")]
        public string CargoDesempeñado { get; set; } = string.Empty;

        public decimal UltimoSalario { get; set; }
        [Required(ErrorMessage = "El campo persona  de contacto es obligatorio")]
        public string PersonaEnCasoDeEmergencia { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Telefono de contacto es obligatorio")]
        public string TelefonoEmergencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo direccion de el contacto es obligatorio")]
        public string DireccionPersonaEmergencia { get; set; } = string.Empty;

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public bool? Activo { get; set; }
    }
}
