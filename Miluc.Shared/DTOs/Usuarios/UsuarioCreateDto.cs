using Miluc.Shared.DTOs.Sap.DireccionCliente;
using Miluc.Shared.DTOs.Sap.Impuesto;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Usuarios
{
    public class UsuarioCreateDto
    {

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        public string Email { get; set; } = string.Empty;


        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Debe contener mayúscula, minúscula, número y carácter especial.")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = string.Empty;

        // Propiedad auxiliar para la carga de imagen en Blazor
        public string? FotoBase64 { get; set; }

        // Propiedad que el servicio guardará en la base de datos
        public byte[]? Foto { get; set; }

        public bool TwoFactorEnabled { get; set; } = true;

        public bool DebeCambiarPassword { get; set; } = false; // Cambiar contraseña en el próximo inicio de sesión


        // Listas de IDs para las tablas intermedias (UsuarioRol y UsuarioTipoUsuario)
        public List<int> RolesIds { get; set; } = new();
        public List<int> TiposUsuarioIds { get; set; } = new();

        // Propiedad auxiliar para el Select simple en la UI
        public int IdTipoUsuario { get; set; }

    }
}

//public class SapClienteCreateDto
//{
//    [Required(ErrorMessage = "Debe seleccionar el tipo de documento.")]
//    public string U_HBT_TipDoc { get; set; } = string.Empty;

//    [Required(ErrorMessage = "El código del cliente es obligatorio.")]
//    [StringLength(20, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 20 caracteres.")]
//    public string CardCode { get; set; } = string.Empty;

//    [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
//    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
//    public string CardName { get; set; } = string.Empty;

//    public string CardType { get; set; } = "C";

//    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un grupo.")]
//    public int GroupCode { get; set; }

//    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una lista de precios.")]
//    public int PriceListNum { get; set; }

//    [Required(ErrorMessage = "El código postal es obligatorio.")]
//    [StringLength(10)]
//    public string ZipCode { get; set; } = string.Empty;

//    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vendedor.")]
//    public int SalesPersonCode { get; set; }

//    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una condición de pago.")]
//    public int PayTermsGrpCode { get; set; }

//    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una prioridad.")]
//    public int Priority { get; set; }

//    [Required(ErrorMessage = "El NIT o documento es obligatorio.")]
//    [RegularExpression(@"^\d{5,15}$",
//        ErrorMessage = "El documento solo puede contener números.")]
//    public string FederalTaxID { get; set; } = string.Empty;

//    [Required(ErrorMessage = "El teléfono es obligatorio.")]
//    [Phone(ErrorMessage = "El teléfono no es válido.")]
//    [StringLength(20)]
//    public string Phone1 { get; set; } = string.Empty;

//    [Phone]
//    public string? Cellular { get; set; }

//    public string Currency { get; set; } = "COP";

//    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
//    [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
//    public string EmailAddress { get; set; } = string.Empty;

//    [Required(ErrorMessage = "El nombre comercial es obligatorio.")]
//    [StringLength(100)]
//    public string CardForeignName { get; set; } = string.Empty;

//    public string? Properties1 { get; set; }
//    public string? Properties2 { get; set; }
//    public string? Properties3 { get; set; }
//    public string? Properties4 { get; set; }
//    public string? Properties5 { get; set; }
//    public string? Properties6 { get; set; }
//    public string? Properties7 { get; set; }
//    public string? Properties8 { get; set; }
//    public string? Properties9 { get; set; }
//    public string? Properties10 { get; set; }

//    public string? U_HBT_RegTrib { get; set; }

//    [Required]
//    public string U_HBT_ActEco { get; set; } = string.Empty;

//    [Required]
//    public string U_HBT_MunMed { get; set; } = string.Empty;

//    [Required]
//    public string U_HBT_TipEnt { get; set; } = string.Empty;

//    [Required]
//    [StringLength(60)]
//    public string U_HBT_Nombres { get; set; } = string.Empty;

//    [Required]
//    [StringLength(60)]
//    public string U_HBT_Apellido1 { get; set; } = string.Empty;

//    [StringLength(60)]
//    public string? U_HBT_Apellido2 { get; set; }

//    [Required]
//    public string U_HBT_Nacional { get; set; } = string.Empty;

//    [Required]
//    public string U_HBT_RegFis { get; set; } = string.Empty;

//    [Required]
//    public string U_HBT_ResFis { get; set; } = string.Empty;

//    [Required]
//    public string U_HBT_ResFis1 { get; set; } = string.Empty;

//    public string? U_HBT_ResFis2 { get; set; }
//    public string? U_HBT_ResFis3 { get; set; }

//    [Required]
//    public string U_HBT_MedPag { get; set; } = string.Empty;

//    [Required]
//    [EmailAddress]
//    public string U_HBT_MailRecep_FE { get; set; } = string.Empty;

//    [Required]
//    [EmailAddress]
//    public string U_addInFaElectronica_email_contacto_FE { get; set; } = string.Empty;

//    [Required]
//    public string U_HBT_Residente { get; set; } = string.Empty;

//    [Required]
//    public string U_HBT_InfoTrib { get; set; } = string.Empty;

//    public string? U_AplicaBolsaMercantil { get; set; }

//    public string Valid { get; set; } = "Y";
//    public string Frozen { get; set; } = "N";

//    [StringLength(254)]
//    public string? FreeText { get; set; }

//    [MinLength(1, ErrorMessage = "Debe registrar al menos una dirección.")]
//    public List<AddressClienteDto> BPAddresses { get; set; } = [];

//    public List<SapBPWithholdingTaxDto> BPWithholdingTaxCollection { get; set; } = [];
//}