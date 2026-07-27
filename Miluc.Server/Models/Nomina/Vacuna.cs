namespace Miluc.Server.Models.Nomina
{
    public class Vacuna
    {

        public int VacunaId { get; set; }
        public string VacunaName { get; set; }  
        public bool Activo { get; set; } 
        public ICollection<EsquemaVacunacion> EsquemaVacunacionModel { get; set; } // Navigation property to EsquemaVacunacionId
    }
}
