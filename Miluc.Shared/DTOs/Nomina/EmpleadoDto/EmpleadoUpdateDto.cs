using Miluc.Shared.DTOs.Nomina.Validations;
using Miluc.Shared.DTOs.Usuarios;
using System;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Nomina.EmpleadoDto
{
    public class EmpleadoUpdateDto
    {
        // Tabla: Empleado

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El documento solo puede contener números.")]
        [MinLength(4, ErrorMessage = "El documento debe tener al menos 4 caracteres.")]
        [MaxLength(20, ErrorMessage = "El documento no puede tener más de 20 caracteres.")]
        public string Documento { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de documento.")]
        public int TipoDocumentoId { get; set; }

        [Required(ErrorMessage = "El identificador del empleado es obligatorio.")]
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        [MinLength(2, ErrorMessage = "El primer nombre debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El primer nombre no puede tener más de 50 caracteres.")]
        public string PrimerNombre { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "El segundo nombre no puede tener más de 50 caracteres.")]
        public string? SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [MinLength(2, ErrorMessage = "El primer apellido debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El primer apellido no puede tener más de 50 caracteres.")]
        public string PrimerApellido { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "El segundo apellido no puede tener más de 50 caracteres.")]
        public string? SegundoApellido { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una empresa.")]
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [MayorDeEdad(ErrorMessage = "El empleado debe ser mayor de 18 años y no debe tener más de 80 años.")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La fecha de expedición del documento es obligatoria.")]
        [FechaExpedicion(ErrorMessage = "La fecha de expedición no es válida.")]
        public DateTime? FechaExpedicionDoc { get; set; }

        [Required(ErrorMessage = "El municipio es obligatorio.")]
        public string CodigoMunicipio { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"(?i)^.+@.+\.(com|co|edu|org|gov|net|com\.co|edu\.co)$", ErrorMessage = "El correo debe tener una extensión válida (.com, .co, .edu, .org, .gov, .net, .com.co o .edu.co).")]
        public string CorreoElectronico { get; set; } = string.Empty;

        
        [RegularExpression(@"^\d{10,}$",ErrorMessage = "El celular debe contener únicamente números y mínimo 10 dígitos.")]
        public string Celular { get; set; } = string.Empty;

        [RegularExpression(@"^\d{10,}$",ErrorMessage = "El celular alterno debe contener únicamente números y mínimo 10 dígitos.")]
        public string? CelularAlterno { get; set; }

        [MaxLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres.")]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "El barrio no puede tener más de 100 caracteres.")]
        public string? Barrio { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado civil.")]
        public int? EstadoCivilId { get; set; }

        // Tabla: ContratoLaboral

        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de contrato.")]
        //public int TipoContratoId { get; set; }

        // Tabla: AfiliacionSeguridadSocial

        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una EPS.")]
        //public int EpsId { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una AFP.")]
        //public int AfpId { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una ARL.")]
        //public int ArlId { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Caja de Compensación.")]
        //public int CajaCompensacionId { get; set; }

        public bool Activo { get; set; }

        public bool AceptaTratamientoDatos { get; set; }

        public DateTime FechaAceptacionTratamientoDatos { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public UsuarioUpdateDto? Usuario { get; set; } = new UsuarioUpdateDto();
    }
}