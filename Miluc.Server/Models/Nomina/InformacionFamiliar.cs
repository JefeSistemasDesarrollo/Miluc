namespace Miluc.Server.Models.Nomina
{
    public class InformacionFamiliar
    {
        public  int InformacionFamiliarId { get; set; }  
        public string Documento { get; set; }
        public int EmpleadoId { get; set; } // Foreign key de Empleado
         public  string NombreCompleto { get; set; }
        public int ParentescoId { get; set; } // Foreign key de Parentesco
        public DateTime FechaNacimiento { get; set; }
        public  bool ViveConEmpleado { get; set; }
        public  bool DependeEconomicamente { get; set; }
        public bool PersonaaCargo { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public Parentesco Parentesco { get; set; } // Navigation a Parentesco
        public Empleado Empleado { get; set; } // Navigation a Empleado
        




    }
}
