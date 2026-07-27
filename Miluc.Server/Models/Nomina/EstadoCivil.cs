using Org.BouncyCastle.Bcpg;

namespace Miluc.Server.Models.Nomina
{
    public class EstadoCivil
    {
        public int EstadoCivilId {  get; set; }
        public string  Nombre { get; set; } 

        public ICollection<Empleado> Empleado {  get; set; }
    }
}
