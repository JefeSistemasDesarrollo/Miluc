namespace Miluc.Shared.DTOs.Nomina.NacionalidadDto
{
    public class NacionalidadReaderDto
    {
        public int NacionalidadId { get; set; }

        public string NacionalidadNombre { get; set; } = string.Empty;

        public string CodigoIso { get; set; } = string.Empty;
    }
}