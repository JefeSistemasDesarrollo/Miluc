namespace Miluc.Server.Models.Nomina
{
    public class Pais
    {
        public int PaisId { get; set; } 
        public string Codigo { get; set; }
        public string pais { get; set; }
        
        public ICollection<MatrizSociodemografica> MatrizSociodemografica {  get; set; }

    }
}
