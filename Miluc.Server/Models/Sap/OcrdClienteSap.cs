using System.Text.Json.Serialization;

namespace Miluc.Server.Models.Sap
{
    public class OcrdClienteSap
    {
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? CardFName { get; set; }
        public string? CardType { get; set; }
        public Int16? GroupCode { get; set; } // FK OCRG GRUPO DELCLIENTE 
        public Int16? ListNum { get; set; } // FK  OPLN  LISTA DE PRECIOS 
        public string? U_HBT_RegTrib { get; set; } // FK  Regimen Tributario
        public string? U_HBT_ResFis { get; set; } // FK  responsabilidad fiscal
        public string? U_HBT_ResFis1 { get; set; } // FK  responsabilidad fiscal
        public string? U_HBT_TipDoc { get; set; } // FK  tipo documento
        public string? U_HBT_MunMed { get; set; } // FK  tipo documento
        public string? U_HBT_RegFis { get; set; } // FK  regimen fiscal
        public int Priority { get; set; } //FK OBPP RUTAS 
        public char ? U_HBT_TipEnt { get; set; }
        public char ? U_HBT_Nacional { get; set; }
        public char ? U_HBT_TipExt { get; set; }
        public string? LicTradNum { get; set; }//RUC O NIT DEL CLIENTE
        public string? Address { get; set; }//DIRECCION PRINCIPAL DEL CLIENTE
        public string? ZipCode { get; set; } //relacion entre ciudad medios magneticos y cliente 
        public string? MailAddres { get; set; } //CORREO ELECTRONICO PRINCIPAL DEL CLIENTE
        public string? Phone1 { get; set; } //TELEFONO PRINCIPAL DEL CLIENTE
        public string? Phone2 { get; set; } //TELEFONO SECUNDARIO DEL CLIENTE
        public Int16? groupNum { get; set; }  //FK OCTG - Condicion de pago
        public char? VatStatus { get; set; }//ESTADO DE IVA DEL CLIENTE
        public int? SlpCode { get; set; } //FK OSLP - Vendedor asignado al cliente
        public string? Currency { get; set; } //moneda del cliente
        public string ? U_HBT_ActEco {  get; set; }//FK HBT_ACTIVIDADECO - Actividad economica del cliente
        public string? Cellular { get; set; } //CELULAR DEL CLIENTE
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
        //public string? FreeText { get; set; } //
        public string? MailBlock { get; set; } //BLOQUEO DE CORREO ELECTRONICO PARA EL CLIENTE
        public string? Password { get; set; } //CONTRASEÑA PARA PORTAL DE CLIENTES (SI SE UTILIZA)
        public char? Deleted { get; set; }
        public int? DocEntry { get; set; } //Campo de entrada de documento en SAP
        public string? U_HBT_Nombres { get; set; }
        public string? U_HBT_Apellido1 { get; set; }
        public string? U_HBT_Apellido2 { get; set; }
        public string? Free_Text { get; set; } //Campo de texto libre para el cliente
        public string? U_HBT_Residente { get; set; } //Campo de texto libre para el cliente
        public string? U_HBT_InfoTrib { get; set; }
        public decimal? Balance { get; set; }
        public string ? U_HBT_MedPag { get; set; } //medios de pago del cliente
        public string ? U_AplicaBolsaMercantil {  get; set; }
        public BusinessPartnerGroup OCRG 
        { 
            get; set;
        } //Grupo de clientes al que pertenece el cliente
        public OPLN OPLN
        { 
            get; set;
        } //Lista de precios asignada al cliente
        public OSLP OSLP
        { 
            get; set; 
        } //Vendedor asignado al cliente
        // Relaciones W¿ENTRE RUTA Y CLIENTE
        public OBPP  OBPP 
        { 
            get; set;
        }  //RUTAS DE ENTREGA

        public OCTG OCTG
        {
            get; set;
        } //Condicion de pago

        //regimentributario 
        public HBT_REGIMTRIB HBT_REGIMTRIB
        {
            get; set;
        }
        public HBT_TIPODOC HBT_TIPODOC
        {
            get; set;
        }
        //actividad economica
        public HBT_ACTIVIDADECO HBT_ACTIVIDADECO
        {
            get; set;
        }
        public HBT_MUNICIPIO HBT_MUNICIPIO
        {
            get; set; 
        }
        //responsabilidad fiscal
        //public OK1_FE_RESPONFIS OK1_FE_RESPONFIS
        //{
        //    get; set;
        //} //Relación entre cliente y responsabilidad fiscal (puede tener una responsabilidad fiscal)


        //referencia regimen fiscal 
        public HBT_REGIMENFISCAL HBT_REGIMENFISCAL 
        { 
            get; set; 

        } //Relación entre cliente y regimen fiscal (puede tener un regimen fiscal)

        //referencia regimen fiscal


        public HBT_RESPFISCAL HBT_RESPFISCAL 
        {
            get; set;
        } //Relación entre cliente y regimen fiscal (puede tener un regimen fiscal)}

        // Relaciones
        public ICollection<CRD1> Direcciones 
        { 
            get; set;
        } //Relación entre cliente y direcciones (puede tener varias direcciones)

        //relacion con precios especiales
        public ICollection<OSPPrecioEspecialSap> OSPPrecioEspecialSap 
        {
            get; set;
        } //Relación entre cliente y precios especiales (puede
        public virtual ICollection<CRD4> CRD4
        {
            get; set;
        }

        [JsonIgnore]
        public ICollection<ORDR> ORDR 
        { 
            get; set;
        } //Relación entre cliente y pedidos (puede tener varios pedidos) 
    }
}
