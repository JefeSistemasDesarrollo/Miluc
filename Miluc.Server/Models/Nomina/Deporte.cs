namespace Miluc.Server.Models.Nomina
{
    public class Deporte
    { 
        
        public  int DeporteId {  get; set; }
        public string Nombre { get; set; }


        public ICollection< MatrizSociodemografica> MatrizSociodemografica {  get; set; }
    }
}
