using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.CajaCompensacionDto
{
    public class CajaUpdate
    {
        public int cajaCompensacionId { get; set; }
        [Required(ErrorMessage = "El nombre de la Caja Compensación es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El código de la Caja Compensación es obligatorio.")]
        public string Codigo { get; set; }  
        public bool Activo { get; set; }
    }
}
