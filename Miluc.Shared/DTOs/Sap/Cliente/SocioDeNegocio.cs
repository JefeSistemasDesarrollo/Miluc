using Miluc.Shared.DTOs.Sap.DireccionCliente;
using Miluc.Shared.DTOs.Sap.Impuesto;
using Miluc.Shared.Models.Validaciones;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Miluc.Shared.DTOs.Sap.Cliente
{
    public class SocioDeNegocio
    {
        //[Required(ErrorMessage = "Debe seleccionar el tipo de documento")]
        [Validar(ErrorMessage = "Debe seleccionar el tipo de documento")]
        public string U_HBT_TipDoc { get; set; }
        public string CardCode { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese la razon social")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre no puede tener entre 3 a 100 caracteres")]
        public string CardName { get; set; }


        // [Required(ErrorMessage = "La sucursal es obligatoria")]
        //[RegularExpression(@"^\d{3}$",ErrorMessage = "La sucursal debe contener exactamente 3 números")]

        //[StringLength(2, MinimumLength = 2, ErrorMessage = "La sucursal debe tener exactamente 2 caracteres")]
        //[StringLength(3,MinimumLength = 3   , ErrorMessage = "La sucursal no puede exceder los 3   caracteres")]

        [Range(0, 999, ErrorMessage = "La sucursal debe ser un número entre 0 y 999")]
        public int SucursalGen { get; set; }

        public string CardType { get; set; }
        [Validar(ErrorMessage = "Debe seleccionar un grupo")]
        public int GroupCode { get; set; }
        [Validar(ErrorMessage = "Debe seleccionar una lista de precios")]
        public int PriceListNum { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el codigo postal es obligatorio.")]

        [StringLength(10)]
        public string? ZipCode { get; set; }

        [Validar(ErrorMessage = "Debe seleccionar un vendedor ")]
        public int SalesPersonCode { get; set; }
        //[Required(ErrorMessage = "Por favor selecciones el metodo de pago ")]
        [Validar(ErrorMessage = "Debe seleccionar un metodo de pago ")]
        public int PayTermsGrpCode { get; set; }
        //[Required(ErrorMessage = "Por favor seleccione la ruta ")]
        [Validar(ErrorMessage = "Debe seleccionar una ruta")]
        public int Priority { get; set; }
        [Required(ErrorMessage = "El NIT o documento es obligatorio.")]
        [RegularExpression(@"^\d{5,15}$", ErrorMessage = "El documento debe contener únicamente números y tener entre 5 y 15 dígitos.")]
        public string FederalTaxID { get; set; }
        public string DigitoVerificacion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\d{3,10}$", ErrorMessage = "Ingrese un teléfono válido (7 a 10 dígitos).")]
        public string Phone1 { get; set; }
        // public string Phone2 { get; set; }
        public string Currency { get; set; }

        [Required(ErrorMessage = "El celular es obligatorio.")]
        [RegularExpression(@"^\d{3,10}$", ErrorMessage = "Ingrese un número celular colombiano válido.")]
        public string Cellular { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "La sucursal es obligatoria.")]
         [StringLength(100, MinimumLength = 0,ErrorMessage = "El nombre comercial debe tener entre 3 y 100 caracteres.")]
        public string CardForeignName { get; set; }

        public string Properties1 { get; set; }
        public string Properties2 { get; set; }
        public string Properties3 { get; set; }
        public string Properties4 { get; set; }
        public string Properties5 { get; set; }
        public string Properties6 { get; set; }
        public string Properties7 { get; set; }
        public string Properties8 { get; set; }
        public string Properties9 { get; set; }
        public string Properties10 { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un regimen Tributario.")]
        public string U_HBT_RegTrib { get; set; }

         [Required(ErrorMessage = "Por favor seleccione una actividad económica.")]
        public string U_HBT_ActEco { get; set; }
        [Required(ErrorMessage = "Por favor seleccione un municipio.")]
        public string U_HBT_MunMed { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una entidad.")]
        public string U_HBT_TipEnt { get; set; }



        //[Required(ErrorMessage = "El nombre es obligatorio.")]
        //[StringLength(60, MinimumLength = 2)]
        //[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$",ErrorMessage = "El nombre solo puede contener letras.")]
        public string U_HBT_Nombres { get; set; }

        //[StringLength(60)]
        //[Required(ErrorMessage = "El primer apellido es obligatorio.")]
        //[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]*$",ErrorMessage = "El primer apellido solo puede contener letras.")]
        public string U_HBT_Apellido1 { get; set; }
        //[Required(ErrorMessage = "Selccione una opcion")]
        //[StringLength(60)]
        //[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]*$",ErrorMessage = "El segundo apellido solo puede contener letras.")]
        public string? U_HBT_Apellido2 { get; set; }
        //[Required(ErrorMessage = "Selccione una opcion")]

        // [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una naionalidad")]
        [Required(ErrorMessage = "Por favor seleccione una nacionalidad.")]
        public string U_HBT_Nacional { get; set; }
        // public string U_HBT_TipExt { get; set; }

        // [Required(ErrorMessage = "Por favor seleccione un regimen fiscal.")]
        public string U_HBT_RegFis { get; set; }
        //[Required(ErrorMessage = "Selccione una opcion")]
        // [Required(ErrorMessage = "Por favor selecicone responsabilidad fiscal ")]
        public string U_HBT_ResFis { get; set; }
        //[Required(ErrorMessage = "Selccione una opcion")]
        //[Required(ErrorMessage = "Por favor seleccione una regimen fiscal.")]
        public string  U_HBT_ResFis1 { get; set; }
        public string U_HBT_ResFis2 { get; set; }
        public string U_HBT_ResFis3 { get; set; }
        //[Required(ErrorMessage = "Selccione una opcion")]

        [Required(ErrorMessage = "Por favor seleccione un medio de pago.")]
        public string U_HBT_MedPag { get; set; }
        public string U_HBT_MailRecep_FE { get; set; }
        public string U_addInFaElectronica_email_contacto_FE { get; set; }

        [Required(ErrorMessage = "Por favor seleccione si es recidente ")]
        public string U_HBT_Residente { get; set; }

        [Required(ErrorMessage = "Por favor seleccione información tributaria ")]
        public string U_HBT_InfoTrib { get; set; }
        //public string U_AplicaBolsaMercantil { get; set; }
        public string Valid { get; set; }
        public string Frozen { get; set; }
        public string FreeText { get; set; }
        public bool U_AplicaBolsaMercantilcheck { get; set; }
        public bool validFor { get; set; } = true;

        //aca voy a llamar la clas de diorecciones del cliente 
        //public List<AddressClienteDto>? BPAddresses { get; set; } = new List<AddressClienteDto> 
        //{
        //    new()
        //    {
        //        AddressName = "Direccion de Factura",
        //        Street = "",
        //        Block = "",
        //        ZipCode = "",
        //        City = "",
        //        County = "",
        //        Country = "",
        //        AddressType = "bo_BillTo",
        //        U_HBT_MunMed = "",
        //        U_HBT_DirMM = ""
        //    }, new()
        //    {
        //        AddressName = "Direccion de Despacho",
        //        Street = "",
        //        Block = "",
        //        ZipCode = "",
        //        City = "",
        //        County = "",
        //        Country = "",
        //        AddressType = "bo_ShipTo",
        //        U_HBT_MunMed = "",
        //        U_HBT_DirMM = ""
        //    }
        //};
        //[ValidateComplexType]






        //direccion de factura 


        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(254, ErrorMessage = "La dirección no puede superar los 254 caracteres.")]
        public string DirreccionFacturaStreet { get; set; }

        [Required(ErrorMessage = "El barrio o bloque es obligatorio.")]
        [StringLength(100, ErrorMessage = "El barrio o bloque no puede superar los 100 caracteres.")]
       public string DirreccionFacturaBlock { get; set; }
        //public string ZipCode { get; set; }
        //public string DireccionFacturaCity { get; set; }
        [Required(ErrorMessage = "El departamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El departamento no puede superar los 100 caracteres.")]
        public string DireccionFacturaCounty { get; set; }
        //public string Country { get; set; }
        //public string AddressType { get; set; }
        //public string U_HBT_MunMed { get; set; }
        //public string U_HBT_DirMM { get; set; }
        //public AddressClienteDto DireccionFactura { get; set; } = new AddressClienteDto();

        //public AddressClienteDto DireccionDespacho { get; set; } = new AddressClienteDto();


        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(254, ErrorMessage = "La dirección no puede superar los 254 caracteres.")]
        public string DirreccionDespachoStreet { get; set; }

        [Required(ErrorMessage = "El barrio o bloque es obligatorio.")]
        [StringLength(100, ErrorMessage = "El barrio o bloque no puede superar los 100 caracteres.")]
        public string DirreccionDespachoBlock { get; set; }
        //public string ZipCode { get; set; }
        //public string DireccionDespachoCity { get; set; }
        [Required (ErrorMessage ="El departamento es obligatio")]
        [StringLength(100, ErrorMessage = "El departamento no puede superar los 100 caracteres.")]
        public string DireccionDespachoCounty { get; set; }




        public List<SapBPWithholdingTaxDto> BPWithholdingTaxCollection { get; set; } = new List<SapBPWithholdingTaxDto>();

    }

   
}
