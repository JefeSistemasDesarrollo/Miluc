using Miluc.Server.Models.Nomina;

public class CondicionMedicaDetalle
{
    public int MatrizSociodemograficaCondicionMedicaId { get; set; }

    public int MatrizSociodemograficaId { get; set; }

    public int CondicionMedicaId { get; set; }

    public string? OtraCondicionMedica { get; set; }

    public virtual MatrizSociodemografica MatrizSociodemografica { get; set; } = null!;

    public virtual CondicionMedica CondicionMedica { get; set; } = null!;
}