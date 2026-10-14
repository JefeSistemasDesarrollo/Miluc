using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EmpresaDto
{
    public class UpsertEmpresaDto
    {
      
        public int EmpresaId { get; set; }

        [Required (ErrorMessage = "Campo nit es obligatorio")]
        public string Nit { get; set; }
        [Required (ErrorMessage ="Campo nombre empresa es obligatorio")]
        public string NombreEmpresa { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

    }
}
