using System;
using System.Collections.Generic;
using System.Text;

namespace Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto
{
    public class AsignarFiltrosCreateDto
    {
        public int AsignarFiltrosId { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }

        public string? NombreModulo { get; set; }
        public string? Descripcion { get; set; }
        public string Filtro { get; set; }

        
    }
}
