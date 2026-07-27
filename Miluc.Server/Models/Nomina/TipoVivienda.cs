namespace Miluc.Server.Models.Nomina
{
    public class TipoVivienda
    {
        public int TipoViviendaId { get; set; }
        public string Nombre { get; set; }


        public ICollection <MatrizSociodemografica> MatrizSociodemografica {  get; set; }
    }
}
