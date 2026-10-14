using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto
{
    public class AsignarFiltroUpdateDto
    {
        public int AsignarFiltrosId { get; set; }
        public int IdUsuario { get; set; }

        //[JsonIgnore]
        //public string NombreUsuario { get; set; }

        public string? NombreModulo { get; set; }
        public string? Descripcion { get; set; }
        public string Filtro { get; set; }

       
    }
}
