using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto
{
    public  class UpdateAFiliacionDto
    {
        public int AfiliacionId { get; set; }
        public int EmpleadoId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una EPS válida.")]
        public int? EpsId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Afp válida.")]
        public int? AfpId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Arl válida.")]
        public int? ArlId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una EPS válida.")]

        public int? CajaCompensacionId { get; set; }
       
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }
    }
}
