namespace Miluc.Server.Models.Nomina
{
    public class NivelAcademico
    {
       public int NivelAcademicoId { get; set; }
       public string Nombre { get; set; }

        public ICollection<MatrizSociodemografica> MatrizSociodemografica {  get; set; }

    }
}
