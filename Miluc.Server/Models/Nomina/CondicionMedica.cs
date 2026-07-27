namespace Miluc.Server.Models.Nomina
{
    public class CondicionMedica
    {
        public int CondicionMedicaId {  get; set; }
        public string Nombre { get; set; }

        public ICollection <MatrizSociodemografica> MatrizSociodemografica { get; set; }
    }
}
