using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto
{
    public class MatrizSocioDemograficaUpdateDto
    {
        [Range(typeof(bool), "true", "true",
            ErrorMessage = "Debe aceptar el consentimiento informado.")]
        public bool ConsentimientoInformado { get; set; }

        public int MatrizSociodemograficaId { get; set; }
        public int EmpleadoId { get; set; }

        public string NombreEmpleado { get; set; } = string.Empty;

        [Range(1, 120, ErrorMessage = "Debe ingresar una edad válida")]
        public int Edad { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una nacionalidad")]
        public int NacionalidadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un país de nacimiento")]
        public int PaisNacimientoId { get; set; }

        public string NombrePais { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un género")]
        public int GeneroId { get; set; }

        public string NombreGenero { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar Sí o No")]
        public bool? Fuma { get; set; }

        [Range(0, 1000, ErrorMessage = "Cantidad inválida")]
        public int FumaVecesAlMes { get; set; }

        [Required(ErrorMessage = "Debe seleccionar Sí o No")]
        public bool? Alcohol { get; set; }

        [Range(0, 1000, ErrorMessage = "Cantidad inválida")]
        public int BebeVecesAlAño { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un deporte")]
        public int DeporteId { get; set; }

        public string NombreDeporte { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar condición médica")]
        public int CondicionMedicaId { get; set; }

        public string NombreCondicionMedica { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Hobbies es obligatorio")]
        [MinLength(1, ErrorMessage = "El campo Hobbies es obligatorio")]
        public string Hobbies { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un medio de transporte")]
        public int MedioDeTransporteId { get; set; }

        public string NombreMedioDeTransporte { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar clase de vivienda")]
        public int ClaseDeViviendaId { get; set; }

        public string NombreClaseVivienda { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar tipo de vivienda")]
        public int TipoDeViviendaId { get; set; }

        public string NombreTipoDeVivienda { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar nivel académico")]
        public int NivelAcademicoId { get; set; }

        public string NombreNivelAcademico { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de finalización de estudios es obligatoria")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "La fecha de culminación de estudios debe ser pasada o presente")]
        public DateTime? AñoFinalizacionEducacion { get; set; }

        [Required(ErrorMessage = "La entidad educativa es obligatoria")]
        [MinLength(1, ErrorMessage = "La entidad educativa es obligatoria")]
        public string EntidadEducativa { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título obtenido es obligatorio")]
        [MinLength(1, ErrorMessage = "El título obtenido es obligatorio")]
        public string TituloObtenido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La última empresa es obligatoria")]
        [MinLength(1, ErrorMessage = "La última empresa es obligatoria")]
        public string UltimaEmpresaTrabajo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha del último empleo es obligatoria")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "La fecha de último empleo debe ser una fecha pasada o presente")]
        public DateTime? FechaUltimoEmpleo { get; set; }

        [Required(ErrorMessage = "El cargo desempeñado es obligatorio")]
        [MinLength(1, ErrorMessage = "El cargo desempeñado es obligatorio")]
        public string CargoDesempeñado { get; set; } = string.Empty;


        [Range(typeof(decimal), "1", "999999999", ErrorMessage = "Debe ingresar un salario válido.")]
        public decimal UltimoSalario { get; set; }

        [Required(ErrorMessage = "La persona de contacto es obligatoria")]
        [MinLength(1, ErrorMessage = "La persona de contacto es obligatoria")]
        public string PersonaEnCasoDeEmergencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono de contacto es obligatorio")]
        [MinLength(1, ErrorMessage = "El teléfono de contacto es obligatorio")]
        public string TelefonoEmergencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección del contacto es obligatoria")]
        [MinLength(1, ErrorMessage = "La dirección del contacto es obligatoria")]
        public string DireccionPersonaEmergencia { get; set; } = string.Empty;

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public bool? Activo { get; set; }
    }
}