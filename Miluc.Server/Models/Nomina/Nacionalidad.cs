namespace Miluc.Server.Models.Nomina
{
    public class Nacionalidad
    {
        public int NacionalidadId { get; set; }

        public string NacionalidadNombre { get; set; } = string.Empty;

        public string CodigoIso { get; set; } = string.Empty;

        public ICollection<MatrizSociodemografica> MatrizSociodemografica { get; set; }
    }
}