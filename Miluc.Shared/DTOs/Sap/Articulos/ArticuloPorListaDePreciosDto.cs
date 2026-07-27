namespace Miluc.Shared.DTOs.Sap.Articulos
{
    public class ArticuloPorListaDePreciosDto
    {
        public string ItemCode { get; set; }//codigo del articulo
        public string? ItemName { get; set; } //nombre del articulo
        public decimal? Quantity { get; set; }   //cantidad del articulo en la lista de precios
        public decimal? SWeight1 { get; set; } //peso bruto del articulo
        public decimal? PriceAcobrar { get; set; }  //precio normal 
        public decimal? PriceEspecial { get; set; }
        public decimal? PriceAsignado { get; set; }
        public string? BuyUnitMsr { get; set; }  //unidad de medida de compra
        public decimal? U_EquivalentedKg { get; set; }  //cantidad de kg por unidad de medida de compra
        public decimal? U_EquivalenteUni { get; set; } //equivalentes de unidades de medida de venta por unidad de medida de compra
        public decimal  BaseSum { get; set; } //precio base del articulo en la lista de precios
        public decimal  Rate { get; set; }
        public string? TaxCode { get; set; }

        public string? CodeTaxcode { get; set; }
        //public decimal? VatPrcnt { get; set; }

       // public string NombreRate { get; set; }
        public decimal TotalImpuesto { get; set; }




    }
}
