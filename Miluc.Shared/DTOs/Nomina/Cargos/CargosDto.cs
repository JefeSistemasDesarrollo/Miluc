using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.Cargos
{
    public class CargosDto
    {
        public int CargoId { get; set; }
        public string CargoNombre { get; set; }
        public bool Activo { get; set; }
    }
}
