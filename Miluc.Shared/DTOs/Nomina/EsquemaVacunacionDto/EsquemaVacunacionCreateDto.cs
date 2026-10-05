using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto
{
    public class CreateEsquemaVacunacionDto
    {

        public int EmpleadoId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una vacuna")]
        public int VacunaId { get; set; }

        public string NombreVacuna { get; set; }
        [Required(ErrorMessage = "La fecha  es obligatoria")]
        [DataType(DataType.Date)]
        [PastOrPresentDate(ErrorMessage = "Fecha vacuna  no puede ser mayor a la fecha actual")]
        public DateTime? FechaVacuna { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }



    }
}
