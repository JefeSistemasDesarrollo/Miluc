using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Miluc.Shared.DTOs.Permisos
{
    public class PermisosCreateDto
    {

        [Required(ErrorMessage ="El campo Nombre es requerido")]
        [MinLength(5,ErrorMessage = "El campo debe tener un minimo de 8 caracteres")]
        [MaxLength(100,ErrorMessage = "El campo debe tener un minimo de 8 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public List<int> RolesIds { get; set; } = new();

        public int IdRol { get; set; }



    }
}
