using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.EmpresaDto
{
    public class EmpresaReaderDto
    {
        public int EmpresaId { get; set; }
        public string Nit { get; set; }
        public string NombreEmpresa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

    }
}
