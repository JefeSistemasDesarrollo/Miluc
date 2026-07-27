namespace Miluc.Server.Models.Sap
{
    public class OSPPrecioEspecialSap
    {
        public string ItemCode { get; set; }//codigo del articulo al que corresponde el precio especial
        public string CardCode { get; set; }//codigo del cliente al que corresponde el precio especial
        public decimal Price { get; set; } = 0;
        public string Currency { get; set; }
        public char? Valid { get; set; } //indica si el precio especial esta activo o inactivo en SAP
        //relacion con la entidad de ocrd cliente sap, un precio especial corresponde a un cliente especifico
        public OcrdClienteSap OcrdClienteSap { get; set; }
        public OITM OITM { get; set; }
    }
}
