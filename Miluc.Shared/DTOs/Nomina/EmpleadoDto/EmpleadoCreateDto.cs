using System;
using System.ComponentModel.DataAnnotations;
using Miluc.Shared.DTOs;
using Miluc.Shared.DTOs.Nomina.Validations;
using Miluc.Shared.DTOs.Usuarios;



namespace Miluc.Shared.DTOs.Nomina.EmpleadoDto
{
    public class EmpleadoCreateDto
    {
        // Tabla: Empleado
        [Required(ErrorMessage = " Campo documento es obligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El documento solo puede contener números")]
        public string Documento { get; set; } = string.Empty;
        [Required(ErrorMessage ="Tipo de documento es obligatorio")]
        public int? TipoDocumentoId { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio")]
        public string PrimerNombre { get; set; } = string.Empty;
        public string? SegundoNombre { get; set; } = string.Empty;


        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        public string PrimerApellido { get; set; } = string.Empty;
        public string? SegundoApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [MayorDeEdad(ErrorMessage = "El empleado debe ser mayor de 18 años y no debe tener mas de 80 años.")]

        public DateTime? FechaNacimiento { get; set; }
        [Required(ErrorMessage = "La fecha de expedición del documento es obligatoria.")]
        [FechaExpedicion(ErrorMessage = "La fecha de expedición no es válida.")]
        public DateTime? FechaExpedicionDoc { get; set; }

       

        [Required(ErrorMessage = "Codigo municipio es obligatorio")]
        public string CodigoMunicipio { get; set; } = string.Empty;

        [RegularExpression(@"^$|^\d{10,15}$", ErrorMessage = "El número de celular debe contener entre 10 y 15 dígitos")]
        public string Celular { get; set; } = string.Empty;

        [RegularExpression(@"^$|^\d{10,15}$", ErrorMessage = "El número de celular alterno debe contener entre 10 y 15 dígitos")]
        public string? CelularAlterno { get; set; } = string.Empty;

        [Required(ErrorMessage = "Correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string? Barrio { get; set; } = string.Empty;
        public bool Activo { get; set; }

        //SELECCIONAR ESTADO CIVIL OBLIGATORIO
        [Required(ErrorMessage = "Estado civil es obligatorio")]
        public int? EstadoCivilId { get; set; }

        // Tabla: ContratoLaboral
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una de Empresa")]
        public int EmpresaId { get; set; }
        public int? TipoContratoId { get; set; }

        // Tabla: AfiliacionSeguridadSocial
        public int EpsId { get; set; }
        public int AfpId { get; set; }
        public int ArlId { get; set; }
        public int CajaCompensacionId { get; set; }

        public UsuarioCreateDto Usuario { get; set; } = new UsuarioCreateDto();
    }
}