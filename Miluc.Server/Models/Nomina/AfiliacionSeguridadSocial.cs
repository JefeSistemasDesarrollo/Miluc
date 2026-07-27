namespace Miluc.Server.Models.Nomina
{
    public class AfiliacionSeguridadSocial
    {
        public int AfiliacionId { get; set; }
        public int EmpleadoId { get; set; }
      
        public int  EpsId { get; set; }
        public int  AfpId { get; set; }
        public int ArlId { get; set; }
        public int CajaCompensacionId  {get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public  bool Activo { get; set; }   

        public Eps? Eps { get; set; }
        public Arl? Arl { get; set; }
        public Afp? Afp { get; set; }
        public CajaCompensacion? CajaCompensacion { get; set; }
        public Empleado? Empleado { get; set; }








    }
}
