namespace Miluc.Server.Models.Sap
{
    public class OPLN
    {
        public Int16 ListNum { get; set; } //Clave primaria de la tabla OPLN, representa el número de lista de precios en SAP
        public string? ListName { get; set; } //Nombre descriptivo de la lista de precios, utilizado para identificarla en SAP
        public Int16? GroupCode { get; set; }//Código del grupo de clientes al que se asigna la lista de precios, utilizado para segmentar clientes en SAP
        public Int16? BASE_NUM { get; set; }//Número de lista de precios base en SAP, utilizado para establecer relaciones entre listas de precios
        public char? ValidFor { get; set; }//Indica si la lista de precios está activa o inactiva en SAP, utilizado para controlar su disponibilidad
        //Relación con la entidad OcrdClienteSap, una lista de precios puede estar asignada a múltiples clientes
        public ICollection<OcrdClienteSap> OCRD { get; set; } = [];
        public ICollection<ITM1> ITM1 { get; set; } = [];//Relación con la entidad ITM1, una lista de precios puede tener múltiples precios de artículos asociados

        //relacion con lista de precios espaciales  

        //Relación con la entidad OSPPrecioEspecialSap, una lista de precios puede tener múltiples precios especiales asociados
        //  public ICollection<OSPPrecioEspecialSap> OSPPrecioEspecialSap { get; set; } = [];

    }
}
