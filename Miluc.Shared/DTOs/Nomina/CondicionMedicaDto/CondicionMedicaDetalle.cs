namespace Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto
{
    public class CondicionMedicaDetalleReaderDto
    {
        public int CondicionMedicaId { get; set; }

        public string NombreCondicionMedica { get; set; } = string.Empty;

        public string? OtraCondicionMedica { get; set; }
    }
}