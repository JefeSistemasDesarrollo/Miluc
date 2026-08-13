using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EpsDto
{
    public class UpdateEpsDto
    {
        public int EpsId { get; set; }
        [Required(ErrorMessage ="Nombre EPS es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Codigo EPS es obligatorio")]
        public string Codigo { get; set; }


        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }
    }
}
