using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto
{
    public class AfiliacionSeguridadSocialreaderDto
    {
        public int AfiliacionId { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; }
        public string Documento { get; set; }
        public int? EpsId { get; set; }
        public string NombreEps { get; set; }   
        public int? AfpId { get; set; }
        public string NombreAfp { get; set; }
        public int? ArlId { get; set; }
        public string NombreArl { get; set; }
        public int? CajaCompensacionId { get; set; }
        public string NombreCajaCompensacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool? Activo { get; set; }
       
    }
}
