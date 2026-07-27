namespace Miluc.Server.Models.Nomina
{
    public class EsquemaVacunacion
    {
        public int EsquemaVacunacionId { get; set; }
        public int EmpleadoId { get; set; }// Foreign key de Empleado
        public int VacunaId { get; set; }// Foreign key de  Vacuna
        public DateTime FechaVacuna { get; set; }
        public int DateTime { get; set; }

        public Vacuna Vacuna { get; set; } // Navigation  a Vacuna 
        public Empleado Empleado { get; set; } // Navigation a Empleado
    }
}
