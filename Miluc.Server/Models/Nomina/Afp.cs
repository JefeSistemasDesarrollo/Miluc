namespace Miluc.Server.Models.Nomina
{
    public class Afp
    {
        public int AfpId { get; set; }
        public string Nombre {  get; set; }
        public string Codigo { get; set; }
   
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get;  set; }
        public bool Activo { get; set; }

        public ICollection<AfiliacionSeguridadSocial> AfiliacionSeguridadSocial {  get; set; }



    }
}
