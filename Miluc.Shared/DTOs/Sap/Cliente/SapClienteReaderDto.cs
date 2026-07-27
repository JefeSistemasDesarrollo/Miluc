using Miluc.Shared.DTOs.Sap.DireccionCliente;
using Miluc.Shared.DTOs.Sap.Impuesto;

namespace Miluc.Shared.DTOs.Sap.Cliente
{
    public class SapClienteReaderDto
    {
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? CardFName { get; set; } //NOMBRE DE FANTASIA DEL CLIENTE  
        public string? ListName { get; set; }
        public string? CardType { get; set; }
        public string? U_HBT_RegTrib { get; set; } // FK  Regimen Tributario
        public string? U_HBT_RegTribName { get; set; } // FK  Regimen Tributario
        public string? U_HBT_TipDoc { get; set; } // FK  tipo de documentocliente 
        public string? U_HBT_TipDocName { get; set; } // FK  tipo de documentocliente 
        public string? HBT_MUNICIPIOName { get; set; } // tipo de documentocliente 
        public string? HBT_MUNICIPIOCode { get; set; } // tipo de documentocliente 
        public string? U_HBT_ActEcoCode { get; set; } // tipo de documentocliente 
        public string? U_HBT_ActEcoName { get; set; } // tipo de documentocliente 
        public string ? U_HBT_MedPag { get; set; } // 
        public string ? U_HBT_Residente { get; set; } // 
        //public string? OK1_FE_RESPONFISCode { get; set; } // tipo de documentocliente 
        public string? U_descripcion { get; set; } // responsabilidad fiscal descripcion

        public string? LicTradNum { get; set; }//RUC O NIT DEL CLIENTE
        public string? Address { get; set; }//DIRECCION PRINCIPAL DEL CLIENTE
        public string? ZipCode { get; set; } //la relacion entre ciudad medios magneticos y cliente
        public string? MailAddres { get; set; } //CORREO ELECTRONICO PRINCIPAL DEL CLIENTE
        public string? Phone1 { get; set; } //TELEFONO PRINCIPAL DEL CLIENTE
        public string? Phone2 { get; set; } //TELEFONO SECUNDARIO DEL CLIENTE
                                            // public Int16? groupNum { get; set; }  //FK OCTG - Condicion de pago
        public char? VatStatus { get; set; }//ESTADO DE IVA DEL CLIENTE
                                            //  public int? slpCode { get; set; } //FK OSLP - Vendedor asignado al cliente
        public string? Currency { get; set; } //moneda del cliente
        public string? Celular { get; set; } //CELULAR DEL CLIENT
        public string? City { get; set; } //CIUDAD DEL CLIENTE  
        public string? County { get; set; } //DEPARTAMENTO DEL CLIENTE
        public string? E_Mail { get; set; } //CORREO ELECTRONICO PRINCIPAL DEL CLIENTE
        public string? U_HBT_MailRecep_FE { get; set; } //CORREO ELECTRONICO DE RECEPCION DE FACTURAS DEL CLIENTE
        public char? QryGroup1 { get; set; }
        public char? QryGroup2 { get; set; }
        public char? QryGroup3 { get; set; }
        public char? QryGroup4 { get; set; }
        public char? QryGroup5 { get; set; }
        public char? QryGroup6 { get; set; }
        public char? QryGroup7 { get; set; }
        public char? QryGroup8 { get; set; }
        public char? QryGroup9 { get; set; }
        public char? QryGroup10 { get; set; }
        public DateTime? CreateDate { get; set; }//FECHA DE CREACION DEL REGISTRO EN SAP
        public DateTime? UpdateDate { get; set; }//FECHA DE ULTIMA ACTUALIZACION DEL REGISTRO EN SAP
        public string? CrCardNum { get; set; }//NUMERO DE TARJETA DE CREDITO DEL CLIENTE
        public char? validFor { get; set; }//INDICA SI EL CLIENTE ESTA ACTIVO O INACTIVO EN SAP
        public string? DebPayAcct { get; set; }//CUENTA CONTABLE DE CLIENTE EN SAP
        public string? shipToDef { get; set; } //ALMACEN DE ENTREGA PREDETERMINADO PARA
        public string? Block { get; set; } //
        public string? Free_Text { get; set; } //Campo de texto libre para el cliente
        public string? MailBlock { get; set; } //BLOQUEO DE CORREO ELECTRONICO PARA EL CLIENTE
        public string? Password { get; set; } //CONTRASEÑA PARA PORTAL DE CLIENTES (SI SE UTILIZA)
        public char? Deleted { get; set; }
        public int? DocEntry { get; set; } //Campo de entrada de documento en SAP
        public string? U_HBT_Nombres { get; set; }
        public string? U_HBT_Apellido1 { get; set; }
        public string? U_HBT_Apellido2 { get; set; }
        public Int16? GroupCode { get; set; }      // FK OCRG
        public string GroupName { get; set; }
        public Int16? ListNum { get; set; } // FK  OPLN
        //public string? ListName { get; set; }
        public int SlpCode { get; set; } //OSLP
        public string SlpName { get; set; }
        public int PrioCode { get; set; } // OBPP   
        public string? PrioDesc { get; set; }
        public Int16 GroupNum { get; set; }
        public string? PymntGroup { get; set; }
        //public string? PymntGroupName { get; set; }
        //public string? Street { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; } = string.Empty;
        public decimal? Balance { get; set; }
        public string? U_HBT_ReSFisCode { get; set; } // FK  responsabilidad fiscal
        public string? U_HBT_ResFisCode1 { get; set; } // FK  responsabilidad fiscal
        public string? U_HBT_regFisCode{ get; set; }
        public string? U_HBT_regFisName{ get; set; }
        public string? U_HBT_resFisName{ get; set; }
        public string? U_HBT_InfoTrib { get; set; }
        public string ? U_HBT_TipEnt { get; set; }
        public string? U_HBT_Nacional { get; set; }
        public char? U_HBT_TipExt { get; set; }


        public string ? U_AplicaBolsaMercantil { get; set; }



        //public int Priority { get; set; } 
        //public string? P { get; set; } // FK  medio de pago


        //public List<string>? Address  { get; set; }
        //aca voy a llamar la clas de diorecciones del cliente 
        public List<DireccionesCrd1ReaderDto>? DireccionPrincipal { get; set; }//calle

        public List<SapBPWithholdingTaxDto> BPWithholdingTaxCollection { get; set; } //retenciones


     
    }
}
