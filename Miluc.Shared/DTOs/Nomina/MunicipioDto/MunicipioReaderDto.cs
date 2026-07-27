using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Nomina.MunicipioDto
{
    public class MunicipioReaderDto
    {
        public string Codigo { get; set; }
        public string CodigoDpto { get; set; }//llave foranea Departamento
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public int EmpleadoId { get; set; }//fk de empleado
        public string NombreDepartamento { get; set; } = string.Empty;






    }
}
