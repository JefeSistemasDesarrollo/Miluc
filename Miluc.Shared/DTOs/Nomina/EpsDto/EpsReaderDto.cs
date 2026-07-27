using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EpsDto
{
    public class EpsReaderDto
    {
        public int EpsId { get; set; }

        public string? Nombre { get; set; }
        public string? Codigo { get; set; }

   

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public bool? Activo { get; set; }
    }
}
