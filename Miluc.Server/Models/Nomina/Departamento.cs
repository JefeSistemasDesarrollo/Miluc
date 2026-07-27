namespace Miluc.Server.Models.Nomina
{
    public class Departamento
    {
       public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

         public ICollection<Municipio> Municipio { get; set; }

    }
}
