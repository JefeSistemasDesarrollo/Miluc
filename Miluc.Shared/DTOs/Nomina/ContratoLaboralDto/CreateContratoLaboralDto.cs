using Miluc.Shared.DTOs.Nomina.Validations;
using System.ComponentModel.DataAnnotations;

public class CreateContratoLaboralDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una empresa.")]
    public int EmpresaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de contrato.")]
    public int TipoContratoId { get; set; }

    public int EmpleadoId { get; set; }

    public string NombreEmpledo { get; set; } = string.Empty;

    public DateTime? FechaCreacion { get; set; }

    public bool Activo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un centro de costo.")]
    public int CentroCostoId { get; set; }

    public string NombreCentroCosto { get; set; } = string.Empty;

    public int ContratoLaboralId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime? FechaInicio { get; set; }

    [FechaNoAnteriorHoy(ErrorMessage = "La fecha de finalización no puede ser anterior a la fecha de inicio.")]
    public DateTime? FechaFinalizacion { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cargo.")]
    public int CargoId { get; set; }

    [Required(ErrorMessage = "El salario es obligatorio.")]
    [Range(typeof(decimal),
        "1750906",
        "999999999",
        ErrorMessage = "El salario debe ser superior al salario mínimo legal vigente y menor a 999.999.999.")]
    public decimal? Salario { get; set; }
}