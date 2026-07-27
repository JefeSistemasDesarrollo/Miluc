using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EmpleadoDto
{
    public class EmpleadoUpdateDto
    {
        // Tabla: Empleado
        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        public int TipoDocumentoId { get; set; }

        [Required(ErrorMessage = "El identificador del empleado es obligatorio.")]
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        public string PrimerNombre { get; set; } = string.Empty;

        public string? SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El primer apellido no puede tener más de 20 caracteres.")]
        [MinLength(2, ErrorMessage = "El primer apellido debe tener al menos 2 caracteres.")]
        public string PrimerApellido { get; set; } = string.Empty;

        public string? SegundoApellido { get; set; }

        
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime? FechaNacimiento { get; set; }

        
        [Required(ErrorMessage = "La fecha de expedición del documento es obligatoria.")]
        public DateTime? FechaExpedicionDoc { get; set; }

        [Required(ErrorMessage = "El municipio es obligatorio.")]
        public string CodigoMunicipio { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; } = string.Empty;

    
        public string Celular { get; set; } = string.Empty;

        public string? CelularAlterno { get; set; }

        
        public string Direccion { get; set; } = string.Empty;

        public string? Barrio { get; set; }

        
        [Required(ErrorMessage = "El estado civil es obligatorio.")]
        public int? EstadoCivilId { get; set; }

        // Tabla: ContratoLaboral
        public int EmpresaId { get; set; }
        public int TipoContratoId { get; set; }

        // Tabla: AfiliacionSeguridadSocial
        public int EpsId { get; set; }
        public int AfpId { get; set; }
        public int ArlId { get; set; }
        public int CajaCompensacionId { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}