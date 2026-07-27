namespace Miluc.Server.Models.Nomina
{
    public class Parentesco
    {
        public int ParentescoId { get; set; }
        public string NombreParentesco { get; set; }

       public ICollection<InformacionFamiliar> InformacionFamiliarModel { get; set; } // Navigation property to InformacionFamiliarId

    }
}
