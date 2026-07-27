namespace Miluc.Server.Models.Nomina
{
    public class ClaseVivienda
    {
        public int ClaseViviendaId { get; set; }
        public string Nombre { get; set; }


        public ICollection<MatrizSociodemografica> MatrizSociodemografica {  get; set; }
    }
}
