namespace Miluc.Server.Models.Nomina
{
    public class Genero
    {
        public int GeneroId { get; set; }
        public string Nombre { get; set; }
        
        public ICollection<MatrizSociodemografica> MatrizSociodemografica {  get; set; }
    }
}
