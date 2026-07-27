namespace Miluc.Server.Models.Sap
{
    public class OK1_PICK_TPLACAS
    {
        public string? code { get; set; }
        public string? Name { get; set; }
        public string? U_Ruta { get; set; }
        public string? U_Sucursal { get; set; }
        public string? U_TipoCamion { get; set; }
        public decimal? U_ValorPorKg { get; set; }
        public string? U_TipoContrato { get; set; }
        public string? U_Conductor { get; set; }

        //aca vamos a agregar la relacion con pedido
        public ICollection<ORDR>? ORDR { get; set; }

       // public ICollection<RDR1>? RDR1 { get; set; }    
    }
}
