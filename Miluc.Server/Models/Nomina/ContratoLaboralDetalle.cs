using Miluc.Server.Models.Nomina;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ContratoLaboralDetalle
    
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //
    public int ContratoLaboralDetalleId { get; set; }
    public int ContratoLaboralId { get; set; }
    public DateTime FechaInicio { get; set; }        // Esta se queda obli
    public DateTime? FechaFinalizacion { get; set; } 
    public DateTime? FechaTerminacion { get; set; }
    public string Observacion { get; set; }
    public int CargoId { get; set; }
    public string CentroCosto { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public ContratoLaboral ContratoLaboral { get; set; }
    public Cargo Cargo { get; set; }
}