using Miluc.Shared.DTOs.Sap.DireccionCliente;
using Miluc.Shared.DTOs.Sap.Impuesto;

namespace Miluc.Shared.DTOs.Sap.Cliente
{
    public class SapClienteCreateEditDto
    {
        public string U_HBT_TipDoc { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public int GroupCode { get; set; }
        public int PriceListNum { get; set; }
        public string? ZipCode { get; set; }
        public int SalesPersonCode { get; set; }
        public int PayTermsGrpCode { get; set; }
         public int Priority { get; set; }
        public string FederalTaxID { get; set; }
        public string Phone1 { get; set; }
        // public string Phone2 { get; set; }
        public string Currency { get; set; }     
        public string Cellular { get; set; }
        public string EmailAddress { get; set; }
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
        public string U_HBT_RegTrib { get; set; }
        public string U_HBT_ActEco { get; set; }
        public string U_HBT_MunMed { get; set; }
        public string U_HBT_TipEnt { get; set; }
        public string U_HBT_Nombres { get; set; } 
        public string U_HBT_Apellido1 { get; set; }
        public string ? U_HBT_Apellido2 { get; set; }
        public string U_HBT_Nacional { get; set; }
        public string U_HBT_RegFis { get; set; }
        public string U_HBT_ResFis { get; set; }
        public string U_HBT_ResFis1 { get; set; }
        public string U_HBT_ResFis2 { get; set; }
        public string U_HBT_ResFis3 { get; set; }
        public string U_HBT_MedPag { get; set; }
        public string U_HBT_MailRecep_FE { get; set; }
        public string U_addInFaElectronica_email_contacto_FE { get; set; }
        public string U_HBT_Residente { get; set; }
        public string U_HBT_InfoTrib { get; set; }
        public string U_AplicaBolsaMercantil { get; set; }
        public string Valid { get; set; }
        public string Frozen { get; set; }
        public string FreeText { get; set; }
        //aca voy a llamar la clas de diorecciones del cliente 
        public List<AddressClienteDto>? BPAddresses { get; set; } = new List<AddressClienteDto>();
        public List<SapBPWithholdingTaxDto> BPWithholdingTaxCollection { get; set; } = new List<SapBPWithholdingTaxDto>();
        // public List<BPPaymentMethodClienteDto>? BPPaymentMethods {  get; set; }
        // public List<BPWithholdingTaxDto>? BPWithholdingTaxCollection {  get; set; }
    }
}
