namespace Miluc.Server.Models.Nomina
{
    public class Municipio
    {
        public string Codigo { get; set; }
        public string CodigoDpto { get; set; }
        public string Nombre { get; set; }
        public  bool Activo { get; set; }

        public int EmpleadoId { get; set; }

        public Departamento Departamento { get; set; }//navegacion a Deartamento
         public ICollection <Empleado> Empleados { get; set; }
    }
}
