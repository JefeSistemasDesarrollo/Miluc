using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Permisos
{
    public class PermisosRolReaderDto
    {

        public int IdPermiso { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        public bool Asignado { get; set; }
    }
}
