using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.AfpDto
{
    public class UpdateAfp
    {
        public int AfpId { get; set; }

        [Required(ErrorMessage = "El nombre de la AFP es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El código de la AFP es obligatorio.")]
        public string Codigo { get; set; }
        public bool Activo { get; set; }
    }
}
