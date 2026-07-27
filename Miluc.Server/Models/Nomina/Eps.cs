namespace Miluc.Server.Models.Nomina
{
    public class Eps

    {
        public int EpsId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set;}
 
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

        //relacion AfiliacionSeguridadSocial

        public ICollection<AfiliacionSeguridadSocial> AfiliacionSeguridadSocial { get; set; }
    }
}
