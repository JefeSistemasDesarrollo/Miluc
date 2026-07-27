using System.Text.Json.Serialization;

namespace Miluc.Server.Models.Sap
{
    public class OITM
    {
        public string ItemCode { get; set; }//codigo del articulo
        public string? ItemName { get; set; } //nombre del articulo
        public string? TaxCodeAR { get; set; } //fk con retenciones 
        public decimal? SWeight1 { get; set; } //peso bruto del articulo
        public Int16? ItmsGrpCod { get; set; }//grupo de articulo
        public Int16? CstGrpCode { get; set; }//grupo de    impuestos
        public char? SellItem { get; set; }//articulo para la venta 
        public char? validFor { get; set; }//articulo valido para uso
        public char? InvntItem { get; set; }//articulo de inventario
        public char? PrchseItem { get; set; }//articulo de compra
        public string? BuyUnitMsr { get; set; } //unidad de medida de compra
        public char? ItemType { get; set; }//tipo de articulo (servicio, producto, etc.)
        public decimal? NumInBuy { get; set; }//cantidad de unidades de compra por unidad de articulo
        public decimal? AvgPrice { get; set; }//precio promedio del articulo
        public ICollection<ITM1> ITM1 { get; set; } = [];
        public ICollection<OSPPrecioEspecialSap> OSPPrecioEspecialSap { get; set; } = [];
        //impuestos  
        public OSTC OSTC { get; set; }

        [JsonIgnore]
        public ICollection<RDR1> RDR1 { get; set; }
    }
}
