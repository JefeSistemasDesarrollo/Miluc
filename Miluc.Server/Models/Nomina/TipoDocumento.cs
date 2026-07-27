using System.Collections;

namespace Miluc.Server.Models.Nomina
{
    public class TipoDocumento
    {
        public int TipoDocumentoId { get; set; }
        public int Codigo{ get; set; }
        public string Nombre { get; set; }

        //public Empleado Empleado { get; set; }
        public ICollection<Empleado> Empleado { get; set; }
    }
}
