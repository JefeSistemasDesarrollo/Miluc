namespace Miluc.Server.Models.Nomina
{
    public class MedioTransporte
    {
        public int MedioTransporteId { get; set; }
         public string Nombre { get; set; } 


        public ICollection<MatrizSociodemografica> MatrizSociodemografica {  get; set; }
    }
}
